using TabpediaFin.Infrastructure.Exceptions;

namespace TabpediaFin.Infrastructure;

public static class StartupExtensions
{
    public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<JwtSettings>(configuration.GetSection(typeof(JwtSettings).Name));
        services.Configure<PasswordHasherSettings>(configuration.GetSection(typeof(PasswordHasherSettings).Name));

        return services;
    }


    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<DbManager>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();

        return services;
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticateRepository, AuthenticateRepository>();

        return services;
    }


    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }

}
