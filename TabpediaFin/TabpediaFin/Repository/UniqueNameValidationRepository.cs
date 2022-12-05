namespace TabpediaFin.Repository;

public class UniqueNameValidationRepository : BaseRepository, IUniqueNameValidationRepository
{
    private readonly ICurrentUser _currentUser;

    public UniqueNameValidationRepository(DbManager dbContext, ICurrentUser currentUser)
    : base(dbContext)
    {
        _currentUser = currentUser;
    }


    public async Task<bool> IsPaymentMethodNameExist(string name, int id)
    {
        return await IsNameExist("PaymentMethod", name, id);
    }


    private async Task<bool> IsNameExist(string tablename, string name, int id)
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

        var sql = @$"SELECT COUNT(1) AS countrow 
            FROM ""{tablename}"" 
            WHERE ""TenantId"" = {tenantId} 
            AND LOWER(REPLACE(""Name"", ' ', '')) = @Name 
            AND ""Id"" <> @Id ";

        var parameters = new DynamicParameters();
        parameters.Add("Name", name.Trim().Replace(" ", "").ToLowerInvariant());
        parameters.Add("Id", id);

        using (var cn = _dbManager.CreateConnection())
        {
            cn.Open();

            var countrow = await cn.ExecuteScalarAsync<int>(sql, parameters);
            return countrow > 0;
        }
    }

}


public interface IUniqueNameValidationRepository
{
    Task<bool> IsPaymentMethodNameExist(string name, int id);
}
