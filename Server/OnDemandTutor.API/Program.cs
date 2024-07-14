using Hangfire;
using Hangfire.Dashboard;
using Mapster;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.API.Extensions;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic;
using OnDemandTutor.BusinessLogic.StartupExtension;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Helper;
using OnDemandTutor.Models;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddLogging();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();
        builder.Services.Configure<SmtpAppSetting>(builder.Configuration.GetSection("SmtpSettings"));
        builder.Services.Configure<VnPay>(builder.Configuration.GetSection("VnPay"));

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
               ));

        // ADD CORS
        builder.Services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
                policyBuilder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));


        builder.Services.AddRepositories()
            .AddGeneralServices()
            .AddFireBaseServices()
            .AddFireBaseHttpClient()
            .AddControllersWithConfiguration()
            .AddCorsWithConfigurations()
            .AddSwaggerWithConfigurations()
            .AddFirebaseAuthentication(builder.Configuration)
            .AddMailConfiguration(builder.Configuration)
            .AddHangFireConfigurations(builder.Configuration);


        builder.Services.AddSignalR();

        // Add global exception handler
        builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAutoMapper(typeof(MapperConfig))
            //.AddAuthenticationService(builder.Configuration)
            .AddMapster();

        // Register Mapster configurations
        var config = TypeAdapterConfig.GlobalSettings;
        new MapsterConfig().Register(config);

        var app = builder.Build();


        // Database connection check
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (!dbContext.Database.CanConnect())
                throw new DatabaseConnectionException($"Cannot connect to the database, current connectionString : {dbContext.Database.GetConnectionString()}");
        }

        // Configure Hangfire
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            DashboardTitle = "OnDemandTutor",
            DarkModeEnabled = true,
            IsReadOnlyFunc = (DashboardContext context) => false,
            TimeZoneResolver = new DefaultTimeZoneResolver()
        });


        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        // Middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseRouting();
        app.UseStatusCodePages();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<ItemHub>("/itemHub");
            endpoints.MapHangfireDashboard();
        });




        app.Run();


    }



}