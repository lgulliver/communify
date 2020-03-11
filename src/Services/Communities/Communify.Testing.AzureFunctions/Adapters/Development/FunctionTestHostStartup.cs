using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Communify.Testing.AzureFunctions.Adapters.Development
{
    public class FunctionTestHostStartup : IStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<FunctionHandlerMiddleware>();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            return services.BuildServiceProvider();
        }
    }
}