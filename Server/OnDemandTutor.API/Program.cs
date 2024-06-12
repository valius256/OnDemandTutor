using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.API.Extensions;
using OnDemandTutor.Helper;
using OnDemandTutor.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        builder.Services.AddControllers();
        builder.Services.AddLogging();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        //builder.Services.UseCore(typeof(Program).Assembly, builder.Configuration);

        builder.Services.AddRepositories()
                        .AddGeneralServices()
                        .AddFireBaseServices()
                        .AddHangFireConfigurations()
                        .AddFireBaseHttpClient()
                        .AddControllersWithConfiguration()
                        .AddCorsWithConfigurations()
                        .AddSwaggerWithConfigurations()
                        ;



        builder.Services.AddAutoMapper(typeof(MapperConfig))
                        .AddAuthenticationService(builder.Configuration);

        builder.Services.AddMapster();

        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!dbContext.Database.CanConnect())
            {
                throw new NotImplementedException("Cannot connect to the database");
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStatusCodePages();
        app.UseExceptionHandler();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        {
            DashboardTitle = "OnDemandTutor",
            DarkModeEnabled = true,
            TimeZoneResolver = new DefaultTimeZoneResolver(),
        });

        app.MapControllers();

        app.Run();
    }
}