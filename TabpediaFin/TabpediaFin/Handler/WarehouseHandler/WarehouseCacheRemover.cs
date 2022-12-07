using TabpediaFin.Infrastructure.Worker;

namespace TabpediaFin.Handler.WarehouseHandler;

public class WarehouseCacheRemover : IWarehouseCacheRemover
{
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ICacheService _cacheService;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

    public WarehouseCacheRemover(ICacheKeyService cacheKeyService
        , ICacheService cacheService
        , BackgroundWorkerQueue backgroundWorkerQueue)
    {
        _cacheKeyService = cacheKeyService;
        _cacheService = cacheService;
        _backgroundWorkerQueue = backgroundWorkerQueue;
    }


    public void RemoveCache()
    {
        var key = _cacheKeyService.GetCacheKey(SelectListCacheNames.Warehouse, "", true);
        _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
        {
            await _cacheService.RemoveAsync(key);
        });
    }
}


public interface IWarehouseCacheRemover
{
    void RemoveCache();
}
