using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnDemandTutor.API.Filter;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.DataAccess.Repository;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using OnDemandTutor.BusinessLogic;
using OnDemandTutor.BusinessLogic.Services.Auth;
using OnDemandTutor.BusinessLogic.Services.User;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.API.Middlesware;

namespace OnDemandTutor.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }

        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            services.AddScoped<IUserServices, UserServices>().AddProblemDetails();
            services.AddScoped<AuthServices, AuthServices>();
          
                services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }


        public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Set to true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            });
            return services;
        }

        public static IServiceCollection AddSwaggerWithConfigurations(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "On Demand Tutor API V1", Version = "V1.0" });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description =
                        @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345example'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>(); // Handles the authorization button
                options.SchemaFilter<DateOnlyDocumentFilter>();
            });
            return services;
        }

        public static IServiceCollection AddControllersWithConfiguration(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            return services;
        }

        public static IServiceCollection AddCorsWithConfigurations(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
            return services;
        }


    }
}
