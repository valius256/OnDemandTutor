using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OnDemandTutor.BusinessLogic.StartupExtension
{
    internal static class CommonServiceCollectionExtentions
    {
        public static IServiceCollection RegisterCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddTransient<IMigration, Migrator>();
            return services;
        }
    }
}