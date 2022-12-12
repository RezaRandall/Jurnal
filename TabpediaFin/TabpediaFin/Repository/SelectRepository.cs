using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        return await FetchSelectList("PaymentMethod", true);
    }


    private async Task<List<SelectResponseDto>> FetchSelectList(string tableName, bool hasActiveColumn)
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

        var activeWhere = string.Empty;
        if (hasActiveColumn)
        {
            activeWhere = @" ""IsActive"" = TRUE AND ";
        }
        var sql = @$"SELECT ""Id"", ""Name"" FROM ""{tableName}"" WHERE {activeWhere} ""TenantId"" = {tenantId} ORDER BY ""Name""";

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
