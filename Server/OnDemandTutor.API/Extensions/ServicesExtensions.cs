using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnDemandTutor.API.Filter;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.BusinessLogic.Services.Auth;
using OnDemandTutor.BusinessLogic.Services.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Enum;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi.OperationFilters;
using System.Security.Claims;
using IMailService = OnDemandTutor.BusinessLogic.Interfaces.Sending.IMailService;
using MailService = OnDemandTutor.BusinessLogic.Services.Sending.MailService;

namespace OnDemandTutor.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
            services.AddProblemDetails();
            return services;
        }

        public static IServiceCollection AddGeneralServices(this IServiceCollection services)
        {
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IJwtProviderServices, JwtProviderServices>();
            services.AddScoped<IFireBaseAuthServices, FirebaseAuthServices>();
            services.AddProblemDetails();
            return services;
        }

        public static IServiceCollection AddFireBaseServices(this IServiceCollection services)
        {
            var firebaseJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase.json");
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(firebaseJsonPath),
            });
            return services;
        }

        public static IServiceCollection AddFireBaseHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IJwtProviderServices, JwtProviderServices>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                client.BaseAddress = new Uri(configuration["Authentication:TokenUri"]);
            });
            return services;
        }

        public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var projectId = configuration["Authentication:project_id"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://securetoken.google.com/{projectId}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{projectId}",
                    ValidateAudience = true,
                    ValidAudience = projectId,
                    ValidateLifetime = true
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Customer", policy => policy.RequireClaim(ClaimTypes.Role, RoleStatus.Customer.ToString()));
                options.AddPolicy("Tutor", policy => policy.RequireClaim(ClaimTypes.Role, RoleStatus.Tutor.ToString()));
                options.AddPolicy("Operator", policy => policy.RequireClaim(ClaimTypes.Role, RoleStatus.Operator.ToString()));
                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, RoleStatus.Admin.ToString()));
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

        public static IServiceCollection AddHangFireConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire((sp, config) =>
            {
                config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true,
                });
            });

            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddMailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection("MailSettings"));
            return services;
        }

        public static void InitializeBackgroundJobs(IServiceProvider services)
        {
            var backgroundJobs = services.GetRequiredService<IBackgroundJobClient>();
            var recurringJobs = services.GetRequiredService<IRecurringJobManager>();
            ConfigureBackgroundJobs(backgroundJobs, recurringJobs);
        }

        public static void ConfigureBackgroundJobs(IBackgroundJobClient backgroundJobs, IRecurringJobManager recurringJobs)
        {
            // Example of enqueuing a job
        }
    }
}
