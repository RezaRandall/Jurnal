namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodDeleteHandler : IDeleteByIdHandler<PaymentMethodDto>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;

    public PaymentMethodDeleteHandler(FinContext db, IPaymentMethodCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }


    public async Task<RowResponse<PaymentMethodDto>> Handle(DeleteByIdRequestDto<PaymentMethodDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<PaymentMethodDto>();

        try
        {
            var paymentMethod = await _context.PaymentMethod.FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (paymentMethod == null || paymentMethod.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            _context.PaymentMethod.Remove(paymentMethod);
            await _context.SaveChangesAsync(cancellationToken);

            _cacheRemover.RemoveCache();

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = new PaymentMethodDto();
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}
