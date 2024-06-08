using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.API.Extensions;
using OnDemandTutor.BusinessLogic;
using OnDemandTutor.Helper;
using OnDemandTutor.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        //builder.Services.UseCore(typeof(Program).Assembly, builder.Configuration);

        builder.Services.AddRepositories()
                                 .AddGeneralServices();

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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}