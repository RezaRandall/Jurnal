﻿using TabpediaFin.Infrastructure.Worker;

namespace TabpediaFin.Handler.SalesOrderHandler;

public class SalesOrderCacheRemover : ISalesOrderCacheRemover
{
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ICacheService _cacheService;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

    public SalesOrderCacheRemover(ICacheKeyService cacheKeyService
        , ICacheService cacheService
        , BackgroundWorkerQueue backgroundWorkerQueue)
    {
        _cacheKeyService = cacheKeyService;
        _cacheService = cacheService;
        _backgroundWorkerQueue = backgroundWorkerQueue;
    }


    public void RemoveCache()
    {
        var key = _cacheKeyService.GetCacheKey(SelectListCacheNames.SalesOrder, "", true);
        _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
        {
            await _cacheService.RemoveAsync(key);
        });
    }
}


public interface ISalesOrderCacheRemover
{
    void RemoveCache();
}