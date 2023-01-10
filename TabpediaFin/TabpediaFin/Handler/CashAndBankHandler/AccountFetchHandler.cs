namespace TabpediaFin.Handler.CashAndBank;

public class AccountFetchHandler : IFetchByIdHandler<AccountFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;
    public AccountFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AccountFetchDto>> Handle(FetchByIdRequestDto<AccountFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<AccountFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                //var row = await cn.FetchAsync<AccountFetchDto>(request.Id, _currentUser);
                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("Id", request.Id);

                var sql = @"SELECT * FROM ""Account"" WHERE ""TenantId"" = @TenantId AND ""Id"" = @Id ";
                var result = await cn.QueryFirstOrDefaultAsync<AccountFetchDto>(sql, parameters);

                response.IsOk = true;
                response.Row = result;
                response.ErrorMessage = string.Empty;
            }
        }
        catch (Exception ex)
        {
            response.IsOk = false;
            response.Row = null;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}

[Table("Account")]
public class AccountFetchDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public int CategoryId { get; set; } = 0;
    public int AccountParentId { get; set; } = 0;
    public int BankId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public double Balance { get; set; } = 0;
    public Boolean IsLocked { get; set; } = false;

}