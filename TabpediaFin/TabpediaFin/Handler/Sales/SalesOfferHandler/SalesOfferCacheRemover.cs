using TabpediaFin.Infrastructure.Worker;

namespace TabpediaFin.Handler.SalesOfferHandler;

public class SalesOfferCacheRemover : ISalesOfferCacheRemover
{
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ICacheService _cacheService;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

    public SalesOfferCacheRemover(ICacheKeyService cacheKeyService
        , ICacheService cacheService
        , BackgroundWorkerQueue backgroundWorkerQueue)
    {
        _cacheKeyService = cacheKeyService;
        _cacheService = cacheService;
        _backgroundWorkerQueue = backgroundWorkerQueue;
    }


    public void RemoveCache()
    {
        var key = _cacheKeyService.GetCacheKey(SelectListCacheNames.SalesOffer, "", true);
        _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
        {
            await _cacheService.RemoveAsync(key);
        });
    }
}


public interface ISalesOfferCacheRemover
{
    void RemoveCache();
}