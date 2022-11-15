using FluentMigrator.Runner;
using System.Reflection;

namespace TabpediaFin.Infrastructure.Migrator;

public static class Startup
{
    public static IServiceCollection AddDbMigrator(this IServiceCollection services)
    {
        services
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(
                rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString("DefaultConnection")
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations().For.EmbeddedResources()
            );

        return services;
    }


    public static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        // Instantiate the runner
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        runner.MigrateUp();
    }
}
