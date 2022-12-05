namespace TabpediaFin.Repository;

public class SelectRepository : BaseRepository, ISelectRepository
{
    private readonly ICurrentUser _currentUser;

    public SelectRepository(DbManager dbContext, ICurrentUser currentUser)
    : base(dbContext)
    {
        _currentUser = currentUser;
    }


    public async Task<List<SelectResponseDto>> FetchPaymentMethodSelectList()
    {
        return await FetchSelectList("PaymentMethod");
    }


    private async Task<List<SelectResponseDto>> FetchSelectList(string tablename)
    {
        if (_currentUser == null)
        {
            throw new ArgumentNullException("Current User is required");
        }

        var tenantId = _currentUser.TenantId;
        if (tenantId == 0)
        {
            throw new ArgumentException("Tenant is not valid");
        }

        var sql = @$"SELECT ""Id"", ""Name"" FROM ""{tablename}"" WHERE ""TenantId"" = {tenantId} ORDER BY ""Name""";

        using (var cn = _dbManager.CreateConnection())
        {
            cn.Open();

            var q = await cn.QueryAsync<SelectResponseDto>(sql);
            return q?.AsList() ?? new List<SelectResponseDto>();
        }
    }
}


public interface ISelectRepository
{
    Task<List<SelectResponseDto>> FetchPaymentMethodSelectList();
}
