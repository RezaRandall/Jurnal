using Microsoft.OpenApi.Models;

namespace TabpediaFin.Infrastructure.OpenApi;

public static class Startup
{
    public static IServiceCollection RegisterSwagger(this IServiceCollection services, string title, string version)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "PLease enter field with word 'Bearer' with space and JWT",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme{ Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id="Bearer"} },
                    new string[]{ }}

                });
        });
        return services;
    }

}
