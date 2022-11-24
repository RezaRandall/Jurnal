using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Handler.PaymentTerm;

public class PaymentTermUpdateHandler : IRequestHandler<PaymentTermUpdateDto, RowResponse<PaymentTermDto>>
{
    private readonly FinContext _context;

    public PaymentTermUpdateHandler(FinContext db)
    {
        _context = db;
    }

    public class CommandValidator : AbstractValidator<PaymentTermUpdateDto>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).MaximumLength(100);
            RuleFor(x => x.TermDays).NotNull().NotEmpty();
            RuleFor(x => x.IsActive).NotNull().NotEmpty();
        }
    }

    public async Task<RowResponse<PaymentTermDto>> Handle(PaymentTermUpdateDto req, CancellationToken cancellationToken)
    {
        var result = new RowResponse<PaymentTermDto>();

        try
        {
            var paymentTerm = await _context.PaymentTerm.FirstAsync(x => x.Id == req.Id, cancellationToken);
            paymentTerm.Name = req.Name;
            paymentTerm.Description = req.Description;
            paymentTerm.TermDays = req.TermDays;
            paymentTerm.IsActive = req.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new PaymentTermDto()
            {
                Id = paymentTerm.Id
                ,Name = paymentTerm.Name
                ,Description = paymentTerm.Description
                ,TermDays = paymentTerm.TermDays
                ,IsActive = paymentTerm.IsActive
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

[Table("PaymentTerm")]
public class PaymentTermUpdateDto : IRequest<RowResponse<PaymentTermDto>>
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TermDays { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
