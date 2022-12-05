namespace TabpediaFin.Infrastructure.Caching;

internal static class Startup
{
    internal static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddScoped<ICacheKeyService, CacheKeyService>();
        services.AddTransient<ICacheService, LocalCacheService>();

        return services;
    }
}