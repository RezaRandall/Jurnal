using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Handler.PaymentTerm;

public class PaymentTermDeleteHandler : IRequestHandler<PaymentTermDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public PaymentTermDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }
    public class CommandValidator : AbstractValidator<PaymentTermDeleteDto>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }

    public async Task<RowResponse<bool>> Handle(PaymentTermDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var paymentTermsData = await _context.PaymentTerm.FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (paymentTermsData != null)
            {
                _context.PaymentTerm.Remove(paymentTermsData);
                result.IsOk = true;
                result.ErrorMessage = "Payment Term with id " + request.Id + " has been deleted";
            }
            if (paymentTermsData == null)
            {
                result.IsOk = false;
                result.ErrorMessage = "Data not found";
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


}

[Table("UnitMeasure")]
public class PaymentTermDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; } = 0;
    public int TenantId { get; set; } = 0;
}
