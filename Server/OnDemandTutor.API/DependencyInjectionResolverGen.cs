using System;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models;

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

