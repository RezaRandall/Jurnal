using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TabpediaFin.Infrastructure.Security;

public static class Startup
{
    public static void AddJwt(this IServiceCollection services)
    {
        services.AddScoped<JwtEventHandlers>();

        var signingCredentials = services.BuildServiceProvider().GetService<IOptions<JwtSettings>>();

        services.Configure<JwtIssuerOptions>(
            options =>
            {
                options.Issuer = signingCredentials!.Value.Issuer;
                options.Audience = signingCredentials.Value.Audience;
                options.SigningCredentials = signingCredentials.Value.SigningCredentials;
            });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingCredentials!.Value.SigningCredentials.Key,
            ValidateIssuer = false,
            ValidateAudience = false,
            //ValidateIssuer = true,
            //ValidIssuer = signingCredentials.Value.Issuer,
            //ValidateAudience = true,
            //ValidAudience = signingCredentials.Value.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(
            x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.EventsType = typeof(JwtEventHandlers);
            });
    }

}
