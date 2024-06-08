using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.BusinessLogic.Services.User;
namespace OnDemandTutor.BusinessLogic.StartupExtension
{
    internal static class ManagerServiceCollectionExtentions
    {
        public static IServiceCollection RegisterManagerServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<IUserServices, UserServices>();
            return services;
        }
    }
}
