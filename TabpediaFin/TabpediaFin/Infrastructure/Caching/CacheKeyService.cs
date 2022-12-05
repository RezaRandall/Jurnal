namespace TabpediaFin.Infrastructure.Caching;

public class CacheKeyService : ICacheKeyService
{
    private readonly ICurrentUser? _currentUser;

    public CacheKeyService(ICurrentUser currentUser) => _currentUser = currentUser;

    public string GetCacheKey(string name, object id, bool includeTenantId = true)
    {
        string tenantId = includeTenantId
            ? _currentUser?.TenantId.ToString() ?? throw new InvalidOperationException("GetCacheKey: includeTenantId set to true and no tenannt available.")
            : "GLOBAL";
        return $"{tenantId}_{name}_{id}";
    }
}


public interface ICacheKeyService
{
    public string GetCacheKey(string name, object id, bool includeTenantId = true);
}