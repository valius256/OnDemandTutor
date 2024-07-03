using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OnDemandTutor.API.Filter;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.FAQ;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.BusinessLogic.Interfaces.Payment;
using OnDemandTutor.BusinessLogic.Interfaces.Subject;
using OnDemandTutor.BusinessLogic.Interfaces.Upload;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.BusinessLogic.Services.Auth;
using OnDemandTutor.BusinessLogic.Services.Blog;
using OnDemandTutor.BusinessLogic.Services.Class;
using OnDemandTutor.BusinessLogic.Services.ConsultationRequest;
using OnDemandTutor.BusinessLogic.Services.FAQ;
using OnDemandTutor.BusinessLogic.Services.Mail;
using OnDemandTutor.BusinessLogic.Services.Payment;
using OnDemandTutor.BusinessLogic.Services.Subject;
using OnDemandTutor.BusinessLogic.Services.Upload;
using OnDemandTutor.BusinessLogic.Services.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.SchedulerJobs;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi.OperationFilters;
using System.Security.Claims;


namespace OnDemandTutor.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<ISlotRepository, SlotRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
        services.AddScoped<IConsultationRequestRepository, ConsultationRequestRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITutorDegreeRepository, TutorDegreeRepository>();
        services.AddScoped<IFAQRepository, FAQRepository>();

        services.AddProblemDetails();
        return services;
    }

    public static IServiceCollection AddGeneralServices(this IServiceCollection services)
    {
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IConsultationRequestService, ConsultationRequestService>();
        services.AddScoped<IFAQService, FAQService>();
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IFirebaseUploadServices, FirebaseUploadServices>(); ;
        services.AddScoped<IFireBaseAuthServices, FirebaseAuthServices>();
        services.AddScoped<IVnPayServices, VnPayServices>();

        services.AddTransient<IMailServices, MailServices>();
        services.AddTransient<IJwtProviderServices, JwtProviderServices>();
        services.AddProblemDetails();
        services.AddLogging();
        return services;
    }

    public static IServiceCollection AddFireBaseServices(this IServiceCollection services)
    {
        var firebaseJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase.json");
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(firebaseJsonPath),
            ProjectId = "ondemandtutor-a049e"
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

    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services,
        IConfiguration configuration)
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
            options.AddPolicy("Customer",
                policy => policy.RequireClaim(ClaimTypes.Role, RoleStatus.Customer.ToString()));
            options.AddPolicy("Tutor", policy => policy.RequireClaim(ClaimTypes.Role, RoleStatus.Tutor.ToString()));
            options.AddPolicy("Operator",
                policy => policy.RequireClaim(ClaimTypes.Role, RoleStatus.Operator.ToString()));
            options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, RoleStatus.Admin.ToString()));
        });

        return services;
    }

    public static IServiceCollection AddSwaggerWithConfigurations(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "On Demand Tutor API V1", Version = "V1.0" });

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
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
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
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
        // Register Hangfire and configure it
        services.AddHangfire(config =>
            config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"),
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                })
        );

        // Register Hangfire server
        services.AddHangfireServer();

        // Register any other required services here
        services.AddTransient<IDefaultScheduleJob, DefaultScheduleJob>();

        return services;
    }

    public static IServiceCollection AddMailConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AppSetting>(configuration.GetSection("SmtpSettings"));
        return services;
    }


}