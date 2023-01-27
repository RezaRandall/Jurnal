using TabpediaFin.Handler.CashAndBank;

namespace TabpediaFin.Handler.CashAndBankCategoryHandler;

public class AccountCashAndBankCategoryDeleteHandler : IDeleteByIdHandler<AccountCashAndBankCategoryFetchDto>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;
    private readonly ICurrentUser _currentUser;

    public AccountCashAndBankCategoryDeleteHandler(FinContext db, ICurrentUser currentUser, IPaymentMethodCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AccountCashAndBankCategoryFetchDto>> Handle(DeleteByIdRequestDto<AccountCashAndBankCategoryFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountCashAndBankCategoryFetchDto>();
        try
        {
            var accountCashAndBankCategoy = await _context.AccountCashAndBankCategory.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (accountCashAndBankCategoy == null || accountCashAndBankCategoy.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            _context.AccountCashAndBankCategory.Remove(accountCashAndBankCategoy);

            await _context.SaveChangesAsync(cancellationToken);
            _cacheRemover.RemoveCache();

            result.IsOk = true;
            result.ErrorMessage = String.Empty;
            result.Row = new AccountCashAndBankCategoryFetchDto();
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }



}
