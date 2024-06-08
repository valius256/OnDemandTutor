using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models;

namespace OnDemandTutor.DataAccess
{
    public static class AppBuilderExtensions
    {
        public static IServiceCollection UseOnDemandTutorDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    opt =>
                    {
                        opt.EnableRetryOnFailure();
                    }
                );
                options.EnableSensitiveDataLogging(true);
            });
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
             services.RegisterDataServices();
            return services;
        }

        private static IServiceCollection RegisterDataServices(this IServiceCollection services)
        {
            var interfaceTypes = GetImplementedOfGenericTypes(typeof(IGenericRepository<>));
            var implementedTypes = GetImplementedOfGenericTypes(typeof(GenericRepository<>));

            foreach (var interfaceType in interfaceTypes)
            {
                var typeName = interfaceType.Name[1..];
                var implementType = implementedTypes.FirstOrDefault(t => interfaceType.IsAssignableFrom(t) && t.Name.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));

                services.AddTransient(interfaceType, implementType);
            }
            return services;
        }

        private static IEnumerable<Type> GetImplementedOfGenericTypes(Type genericType)
        {
            var baseEntity = typeof(IBaseEntity);

            return typeof(AppBuilderExtensions)
                .Assembly
                .DefinedTypes
                .Where(t =>
                    t.IsInterface == genericType.IsInterface &&
                    t != genericType &&
                    ((genericType.IsInterface && t.GetInterfaces().Any(i =>
                        i.Namespace.Equals(genericType.Namespace, StringComparison.InvariantCultureIgnoreCase) &&
                        i.Name.Equals(genericType.Name, StringComparison.InvariantCultureIgnoreCase) &&
                        baseEntity.IsAssignableFrom(i.GenericTypeArguments.FirstOrDefault())))
                        || (!genericType.IsInterface &&
                            t.BaseType.Namespace.Equals(genericType.Namespace, StringComparison.InvariantCultureIgnoreCase) &&
                            t.BaseType.Name.Equals(genericType.Name, StringComparison.InvariantCultureIgnoreCase) &&
                            baseEntity.IsAssignableFrom(t.BaseType.GenericTypeArguments.FirstOrDefault()))
                ));
        }


        public static async Task DatabaseMigrateAsync(this IServiceProvider serviceProvider)
        {
            var database = serviceProvider.GetRequiredService<ApplicationDbContext>();
            await database.Database.MigrateAsync();
        }

    }
}
