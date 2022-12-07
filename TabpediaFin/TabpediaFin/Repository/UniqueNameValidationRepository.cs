namespace TabpediaFin.Repository;

public class UniqueNameValidationRepository : BaseRepository, IUniqueNameValidationRepository
{
    private readonly ICurrentUser _currentUser;

    public UniqueNameValidationRepository(DbManager dbContext, ICurrentUser currentUser)
    : base(dbContext)
    {
        _currentUser = currentUser;
    }

    public async Task<bool> IsContactAddressTypeNameExist(string name, int id)
    {
        return await IsNameExist("ContactAddressType", name, id);
    }

    public async Task<bool> IsContactGroupNameExist(string name, int id)
    {
        return await IsNameExist("ContactGroup", name, id);
    }
    
    public async Task<bool> IsExpenseCategoryNameExist(string name, int id)
    {
        return await IsNameExist("ExpenseCategory", name, id);
    }

    public async Task<bool> IsItemCategoryNameExist(string name, int id)
    {
        return await IsNameExist("ItemCategory", name, id);
    }

    public async Task<bool> IsPaymentMethodNameExist(string name, int id)
    {
        return await IsNameExist("PaymentMethod", name, id);
    }

    public async Task<bool> IsTagNameExist(string name, int id)
    {
        return await IsNameExist("Tag", name, id);
    }


    public async Task<bool> IsTaxNameExist(string name, int id)
    {
        return await IsNameExist("Tax", name, id);
    }

    public async Task<bool> IsWarehouseNameExist(string name, int id)
    {
        return await IsNameExist("Warehouse", name, id);
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
    Task<bool> IsContactAddressTypeNameExist(string name, int id);
    Task<bool> IsContactGroupNameExist(string name, int id);
    Task<bool> IsExpenseCategoryNameExist(string name, int id);
    Task<bool> IsItemCategoryNameExist(string name, int id);
    Task<bool> IsPaymentMethodNameExist(string name, int id);
    Task<bool> IsTagNameExist(string name, int id); 
    Task<bool> IsTaxNameExist(string name, int id);
    Task<bool> IsWarehouseNameExist(string name, int id);
}
