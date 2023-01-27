using TabpediaFin.Dto.Common.Request;

namespace TabpediaFin.Handler.PaymentTerm;

public class PaymentTermDeleteHandler : IDeleteByIdHandler<PaymentTermDto>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;
    private readonly ICurrentUser _currentUser;

    public PaymentTermDeleteHandler(FinContext db, IPaymentMethodCacheRemover cacheRemover, ICurrentUser currentUser)
    {
        _context = db;
        _cacheRemover = cacheRemover;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<PaymentTermDto>> Handle(DeleteByIdRequestDto<PaymentTermDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<PaymentTermDto>();
        try
        {
            var paymentTermsData = await _context.PaymentTerm.FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (paymentTermsData == null )
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            _context.PaymentTerm.Remove(paymentTermsData);
            await _context.SaveChangesAsync(cancellationToken);
            _cacheRemover.RemoveCache();
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


}

