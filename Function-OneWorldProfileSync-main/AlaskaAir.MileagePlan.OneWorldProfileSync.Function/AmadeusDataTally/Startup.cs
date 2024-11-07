using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AmadeusDataTally
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var config
                = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();

            services.AddSingleton(config);

            services.AddSingleton<AmadeusOperations>();

            services.AddSingleton<IHttpRequestHelper, HttpRequestHelper>();

            services.AddSingleton<IAmadeusRequestCreationHelper, AmadeusRequestCreationHelper>();

            services.AddHttpClient();

            services.AddSingleton<IValidator, Validator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //});
        }
    }
}
