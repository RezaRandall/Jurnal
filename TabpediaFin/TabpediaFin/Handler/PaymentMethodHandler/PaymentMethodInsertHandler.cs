namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodInsertHandler : IRequestHandler<PaymentMethodInsertDto, RowResponse<PaymentMethodDto>>
{
    private readonly FinContext _context;

    public PaymentMethodInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<PaymentMethodDto>> Handle(PaymentMethodInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<PaymentMethodDto>();

        var paymentMethod = new PaymentMethod()
        {
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive,
        };

        try
        {
            await _context.PaymentMethod.AddAsync(paymentMethod, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new PaymentMethodDto()
            {
                Id = paymentMethod.Id,
                Name = paymentMethod.Name,
                Description = paymentMethod.Description,
                IsActive = paymentMethod.IsActive,
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



public class PaymentMethodInsertDto : IRequest<RowResponse<PaymentMethodDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
