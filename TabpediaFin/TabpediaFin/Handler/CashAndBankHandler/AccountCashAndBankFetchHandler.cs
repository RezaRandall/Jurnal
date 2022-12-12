using TabpediaFin.Handler.ExpenseCategoryHandler;

namespace TabpediaFin.Handler.CashAndBank;

public class AccountCashAndBankFetchHandler : IFetchByIdHandler<AccountCashAndBankFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;
    public AccountCashAndBankFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AccountCashAndBankFetchDto>> Handle(FetchByIdRequestDto<AccountCashAndBankFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<AccountCashAndBankFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<AccountCashAndBankFetchDto>(request.Id, _currentUser);

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

[Table("AccountCashAndBank")]
public class AccountCashAndBankFetchDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public int CashAndBankCategoryId { get; set; } = 0;
    public int DetailAccountId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    public int BankId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public Int64 Balance { get; set; } = 0;
}