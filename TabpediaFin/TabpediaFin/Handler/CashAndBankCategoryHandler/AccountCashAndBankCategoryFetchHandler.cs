namespace TabpediaFin.Handler.CashAndBankCategoryHandler;

public class AccountCashAndBankCategoryFetchHandler : IFetchByIdHandler<AccountCashAndBankCategoryFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;
    public AccountCashAndBankCategoryFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AccountCashAndBankCategoryFetchDto>> Handle(FetchByIdRequestDto<AccountCashAndBankCategoryFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<AccountCashAndBankCategoryFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<AccountCashAndBankCategoryFetchDto>(request.Id, _currentUser);

                response.IsOk = true;
                response.Row = row;
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

[Table("AccountCashAndBankCategory")]
public class AccountCashAndBankCategoryFetchDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
}