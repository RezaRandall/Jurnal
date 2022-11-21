namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodUpdateHandler : IRequestHandler<PaymentMethodUpdateDto, RowResponse<PaymentMethodDto>>
{
    private readonly FinContext _context;

    public PaymentMethodUpdateHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<PaymentMethodDto>> Handle(PaymentMethodUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<PaymentMethodDto>();

        try
        {
            var paymentMethod = await _context.PaymentMethod.FirstAsync(x => x.Id == request.Id, cancellationToken);
            paymentMethod.Name = request.Name;
            paymentMethod.Description = request.Description;
            paymentMethod.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new PaymentMethodDto()
            {
                Name = paymentMethod.Name,
                Description = paymentMethod.Description,
                IsActive = paymentMethod.IsActive
            };

            result.IsOk= true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk= false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}


public class PaymentMethodUpdateDto : IRequest<RowResponse<PaymentMethodDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
