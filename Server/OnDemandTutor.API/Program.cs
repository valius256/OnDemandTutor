using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnDemandTutor.API.Extensions;
using OnDemandTutor.BusinessLogic;
using OnDemandTutor.DataAccess.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    options.AddSecurityDefinition("oath2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authorization",
        //Description = "Bearer {token}"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddCors();
builder.Services.DIServices();
builder.Services.AddLogging();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);


builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<OnDemandTutorContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<OnDemandTutorContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddTransient<ExceptionMiddleware>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
static void UseSwagger(IApplicationBuilder app)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "v1";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "On Demand Tutor API");
    });
}

UseSwagger(app);

//app.UseAuthentication();
//app.UseAuthorization();

// for auto migration
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<OnDemandTutorContext>();
//    db.Database.Migrate();
//    Console.WriteLine("Database Migrated by Phats");
//}
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed((origin) =>
    {
        var hosts = configuration["AllowedHosts"].Split(";").ToList();
        return hosts.Any(h => origin.ToLower().Contains(h.ToLower()));
    })
    .WithExposedHeaders("*")
    .AllowCredentials());
app.UseHttpsRedirection();

app.MapIdentityApi<User>();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();
