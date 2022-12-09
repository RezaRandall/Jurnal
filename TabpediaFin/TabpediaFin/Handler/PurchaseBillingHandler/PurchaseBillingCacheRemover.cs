using TabpediaFin.Infrastructure.Worker;

namespace TabpediaFin.Handler.PurchaseBillingHandler;

public class PurchaseBillingCacheRemover : IPurchaseBillingCacheRemover
{
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ICacheService _cacheService;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

    public PurchaseBillingCacheRemover(ICacheKeyService cacheKeyService
        , ICacheService cacheService
        , BackgroundWorkerQueue backgroundWorkerQueue)
    {
        _cacheKeyService = cacheKeyService;
        _cacheService = cacheService;
        _backgroundWorkerQueue = backgroundWorkerQueue;
    }


    public void RemoveCache()
    {
        var key = _cacheKeyService.GetCacheKey(SelectListCacheNames.PurchaseBilling, "", true);
        _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
        {
            await _cacheService.RemoveAsync(key);
        });
    }
}


public interface IPurchaseBillingCacheRemover
{
    void RemoveCache();
}

