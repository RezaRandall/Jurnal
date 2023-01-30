using TabpediaFin.Handler.CashAndBank;

public class AccountDeleteHandler : IDeleteByIdHandler<AccountFetchDto>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;
    private readonly ICurrentUser _currentUser;

    public AccountDeleteHandler(FinContext db, ICurrentUser currentUser, IPaymentMethodCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AccountFetchDto>> Handle(DeleteByIdRequestDto<AccountFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AccountFetchDto>();
        try
        {
            var accountCashAndBank = await _context.Account.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (accountCashAndBank == null || accountCashAndBank.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            _context.Account.Remove(accountCashAndBank);

            await _context.SaveChangesAsync(cancellationToken);
            _cacheRemover.RemoveCache();

            result.IsOk = true;
            result.ErrorMessage = String.Empty;
            result.Row = new AccountFetchDto();
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}