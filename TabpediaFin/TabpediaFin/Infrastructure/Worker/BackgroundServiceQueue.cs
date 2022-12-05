namespace TabpediaFin.Infrastructure.Worker;

public class BackgroundServiceQueue : BackgroundService
{
    private readonly BackgroundWorkerQueue _queue;

    public BackgroundServiceQueue(BackgroundWorkerQueue queue)
    {
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _queue.DequeueAsync(stoppingToken);

            await workItem(stoppingToken);
        }
    }
}
