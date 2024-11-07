using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;

namespace AmadeusDataTally
{
    public class Program
    {
        private static readonly string connectionString = 
            $"User Id={Environment.GetEnvironmentVariable("dbUserName")};Password={Environment.GetEnvironmentVariable("dbUserPassword")};Data Source={Environment.GetEnvironmentVariable("databaseSource")}";

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var operationUnderTest = Environment.GetEnvironmentVariable("OperationUnderTest");

            var ValidOperations = new List<string> { "Upgrade", "Downgrade" };

            if (!ValidOperations.Contains(operationUnderTest))
            {
                throw(new InvalidDataException($"Environment variable OperationUnderTest should be either {ValidOperations[0]} or {ValidOperations[1]}"));
            }

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            using StreamWriter dataTallyFileWriter = new StreamWriter($"AmadeusDataTally{operationUnderTest}{DateTime.Now:mdyyyyHHss}.csv");
            await dataTallyFileWriter.WriteLineAsync($"Error,MileagePlanNumber");

            AmadeusOperations amadeusOperations = provider.GetRequiredService<AmadeusOperations>();
            IAmadeusRequestCreationHelper amadeusRequestCreationHelper = provider.GetRequiredService<IAmadeusRequestCreationHelper>();

            using OracleConnection con = new OracleConnection(connectionString);
            using OracleCommand cmd = con.CreateCommand();
            try
            {
                con.Open();

                cmd.CommandText = operationUnderTest == ValidOperations[0] ?
                    @"SELECT MEM_NUM,PR_DOM_TIER_ID
                        FROM siebel.s_loy_member
                        WHERE PR_DOM_TIER_ID <> '1-161X'
                        AND status_cd         = 'Active'
                        AND MEM_TYPE_CD       = 'Individual'"
                    :
                    @"SELECT mem.row_id, mem.PR_DOM_TIER_ID
                        FROM siebel.s_loy_mem_tier mt
                        INNER JOIN SIEBEL.S_LOY_TIER tier
                        ON mt.TIER_ID= tier.ROW_ID
                        INNER JOIN siebel.s_loy_member mem
                        ON mem.row_id= mt.MEMBER_ID 
                        INNER JOIN SIEBEL.S_LOY_TIER_CLS tierclass
                        ON TIERCLASS.ROW_ID = TIER.TIER_CLASS_ID
                        WHERE tierclass.NAME= 'Default'
                        AND mt.active_flg   = 'Y'
                        AND mem.STATUS_CD   = 'Active'
                        AND mt.tier_id      ='1-161X'
                        AND mt.SEQ_NUM      > 2
                        AND MT.START_DT     > To_date('12-11-2020', 'mm-dd-yyyy')
                        AND MT.START_DT    <> TO_DATE ('01-01-2021', 'mm-dd-yyyy')";

                OracleDataReader reader = cmd.ExecuteReader();

                Session session = await amadeusOperations.Authenticate();

                var count = 0;
                var iteration = 0;

                while (reader.Read())
                {
                    count++;

                    var memberNumber = reader.GetString(0);
                    var tierId = reader.GetString(1);

                    var profilePublisherRequest = new ProfilePublisherRequest
                    {
                        MileagePlanNumber = reader.GetString(0)
                    };

                    amadeusRequestCreationHelper.GenerateProfileRetrieveRequest(session, profilePublisherRequest);

                    AMA_ProfileReadRS profileReadRS = new AMA_ProfileReadRS();

                    try
                    {
                        profileReadRS = await amadeusOperations.ProfileRetrieve(session, profilePublisherRequest);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"{memberNumber} - Exception calling retrieve {ex.InnerException}");
                    }

                    if (operationUnderTest == ValidOperations[0] && profileReadRS == default)
                    {
                        await dataTallyFileWriter.WriteLineAsync($"NotFound,{memberNumber},{tierId}");
                    }
                    else if(operationUnderTest == ValidOperations[1] && profileReadRS != default)
                    {
                        await dataTallyFileWriter.WriteLineAsync($"Found,{memberNumber},{tierId}");
                    }

                    if (count >= 3000)
                    {
                        iteration++;
                        await dataTallyFileWriter.FlushAsync();
                        await dataTallyFileWriter.WriteLineAsync($"Processed,{count * iteration}");
                        await dataTallyFileWriter.WriteLineAsync($"NumberOfIterations,{iteration}");
                        count = 0;
                    }
                }

                var processedCount = iteration > 0 ? count * iteration : count;

                await dataTallyFileWriter.WriteLineAsync($"Processed,{processedCount}");

                await amadeusOperations.SignOut(session);

                await dataTallyFileWriter.FlushAsync();

                dataTallyFileWriter.Close();

                reader.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception running the query - {ex.InnerException}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             });
    }
}
