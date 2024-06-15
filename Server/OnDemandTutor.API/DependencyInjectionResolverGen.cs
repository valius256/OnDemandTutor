using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.DataAccess.Repository;

namespace OnDemandTutor.API
{
    public static class DependencyInjectionResolverGen
    {
        public static void InitializerDependencyInjection(this IServiceCollection services)
        {
            //user
            services.AddScoped<IUserRepository, UserRepository>();
            //Subject
            services.AddScoped<ISubjectRepository, SubjectRepository>();

        }
    }

}