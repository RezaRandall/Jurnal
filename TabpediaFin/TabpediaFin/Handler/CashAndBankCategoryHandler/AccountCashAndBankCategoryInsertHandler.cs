using TabpediaFin.Handler.CashAndBank;

namespace TabpediaFin.Handler.CashAndBankCategoryHandler;

public class AccountCashAndBankCategoryInsertHandler : IRequestHandler<AccountCashAndBankCategoryInsertDto, RowResponse<AccountCashAndBankCategoryFetchDto>>
{
    private readonly FinContext _context;

    public AccountCashAndBankCategoryInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<AccountCashAndBankCategoryFetchDto>> Handle(AccountCashAndBankCategoryInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountCashAndBankCategoryFetchDto>();

        var accountCashAndBankCategory = new AccountCashAndBankCategory()
        {
            Name = request.Name
        };

        try
        {
            await _context.AccountCashAndBankCategory.AddAsync(accountCashAndBankCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new AccountCashAndBankCategoryFetchDto()
            {
                Id = accountCashAndBankCategory.Id,
                Name = accountCashAndBankCategory.Name,
            };

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


}

public class AccountCashAndBankCategoryInsertDto : IRequest<RowResponse<AccountCashAndBankCategoryFetchDto>>
{
    public string Name { get; set; } = string.Empty;

}
