using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Startup))]

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config
                = new ConfigurationBuilder()
                    .AddEnvironmentVariables()                    
                    .Build();

            builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddSingleton(config);

            builder.Services.AddTransient<OneWorldProfileSyncProcessor>();

            builder.Services.AddSingleton<AmadeusOperations>();

            builder.Services.AddSingleton<IHttpRequestHelper, HttpRequestHelper>();

            builder.Services.AddSingleton<IAmadeusRequestCreationHelper, AmadeusRequestCreationHelper>();

            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IValidator, Validator>();
        }
    }
}
