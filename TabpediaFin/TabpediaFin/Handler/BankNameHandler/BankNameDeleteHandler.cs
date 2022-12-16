using TabpediaFin.Handler.CashAndBank;

namespace TabpediaFin.Handler.BankNameHandler;

public class BankNameDeleteHandler : IDeleteByIdHandler<BankNameFetchDto>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;

    public BankNameDeleteHandler(FinContext db, ICurrentUser currentUser, IPaymentMethodCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<BankNameFetchDto>> Handle(DeleteByIdRequestDto<BankNameFetchDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<BankNameFetchDto>();
        try
        {
            var accountCashAndBankCategoy = await _context.BankName.FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (accountCashAndBankCategoy == null || accountCashAndBankCategoy.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            _context.BankName.Remove(accountCashAndBankCategoy);

            await _context.SaveChangesAsync(cancellationToken);
            _cacheRemover.RemoveCache();

            result.IsOk = true;
            result.ErrorMessage = String.Empty;
            result.Row = new BankNameFetchDto();
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}
