using TabpediaFin.Handler.CashAndBank;

public class AccountCashAndBankDeleteHandler : IDeleteByIdHandler<AccountCashAndBankFetchDto>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;

    public AccountCashAndBankDeleteHandler(FinContext db, ICurrentUser currentUser, IPaymentMethodCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<AccountCashAndBankFetchDto>> Handle(DeleteByIdRequestDto<AccountCashAndBankFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountCashAndBankFetchDto>();
        try
        {
            var accountCashAndBank = await _context.AccountCashAndBank.FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (accountCashAndBank == null || accountCashAndBank.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            _context.AccountCashAndBank.Remove(accountCashAndBank);

            await _context.SaveChangesAsync(cancellationToken);
            _cacheRemover.RemoveCache();

            result.IsOk = true;
            result.ErrorMessage = String.Empty;
            result.Row = new AccountCashAndBankFetchDto();
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}