using TabpediaFin.Infrastructure.Worker;

namespace TabpediaFin.Handler.ContactAddressHandler;

public class ContactAddressCacheRemover : IContactAddressCacheRemover
{
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ICacheService _cacheService;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

    public ContactAddressCacheRemover(ICacheKeyService cacheKeyService
        , ICacheService cacheService
        , BackgroundWorkerQueue backgroundWorkerQueue)
    {
        _cacheKeyService = cacheKeyService;
        _cacheService = cacheService;
        _backgroundWorkerQueue = backgroundWorkerQueue;
    }


    public void RemoveCache()
    {
        var key = _cacheKeyService.GetCacheKey(SelectListCacheNames.ContactAddress, "", true);
        _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
        {
            await _cacheService.RemoveAsync(key);
        });
    }
}


public interface IContactAddressCacheRemover
{
    void RemoveCache();
}
