using Communify.Communities.ApiFunctions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Communify.Communities.ApiFunctions
{
        public class Startup : FunctionsStartup
        {
            public override void Configure(IFunctionsHostBuilder builder)
            {
                ConfigureExternalServices(builder.Services);
            }

            protected virtual void ConfigureExternalServices(IServiceCollection services)
            {
                // services.AddScoped<ICommunityRepository, CommunityRepository>();
                services.AddSingleton(NullLoggerFactory.Instance.CreateLogger("Null Logger"));
            }
        }
    }

