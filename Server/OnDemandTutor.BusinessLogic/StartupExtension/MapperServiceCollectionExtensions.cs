using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OnDemandTutor.BusinessLogic.StartupExtension
{
    public static class MapperServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMapperServices(this IServiceCollection services, Assembly assembly)
        {
            //services.AddAutoMapper(assembly, typeof(MapperServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
