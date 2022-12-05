namespace TabpediaFin.Controllers;

[Route("api/select")]
public class SelectController : ApiControllerBase
{
    private readonly ISelectRepository _selectRepository;
    private readonly ICacheService _cache;
    private readonly ICacheKeyService _cacheKeys;

    public SelectController(ISelectRepository selectRepository,
        ICacheService cache,
        ICacheKeyService cacheKeys)
    {
        _selectRepository = selectRepository;
        _cache = cache;
        _cacheKeys = cacheKeys;
    }


    [HttpGet]
    [Route("payment-method")]
    public async Task<List<SelectResponseDto>> FetchPaymentMethodSelectList()
    {
        var cacheKey = _cacheKeys.GetCacheKey(SelectListCacheNames.PaymentMethod, string.Empty, true);
        var list = await _cache.GetOrSetAsync(cacheKey,
            async () =>
            {
                return await _selectRepository.FetchPaymentMethodSelectList();
            });

        return list ?? new List<SelectResponseDto>();
    }
}
