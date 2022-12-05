namespace TabpediaFin.Infrastructure.Worker;

public static class Startup
{
    public static void AddWorker(this IServiceCollection services)
    {
        services.AddSingleton<BackgroundWorkerQueue>();
        services.AddHostedService<BackgroundServiceQueue>();
    }
}
