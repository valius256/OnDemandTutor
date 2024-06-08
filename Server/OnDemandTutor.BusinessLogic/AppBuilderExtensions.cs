using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnDemandTutor.BusinessLogic.StartupExtension;
using OnDemandTutor.DataAccess;
using System.Reflection;

namespace OnDemandTutor.BusinessLogic
{
    public static class AppBuilderExtensions
    {

        public static void UseCore(this IServiceCollection services, Assembly assembly, IConfiguration configuration)
        {
            services.UseOnDemandTutorDb(configuration)
            .RegisterCommonServices(configuration)
            .RegisterMapperServices(assembly)
            .RegisterManagerServices(configuration);
        }
    }
}
