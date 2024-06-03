using Microsoft.OpenApi.Models;

namespace OnDemandTutor.API.Extensions
{
    public static class ServiceExtensions
    {
        //public static void InitSwagger(this IServiceCollection services) =>
            //services.AddSwaggerGen(c =>
            //{
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Description = "JWT Authorization header using the Bearer scheme."
            //    });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] {}
            //        }
            //    });
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "On Demand Tutor API", Version = "v1" });
            //});

    }
}
