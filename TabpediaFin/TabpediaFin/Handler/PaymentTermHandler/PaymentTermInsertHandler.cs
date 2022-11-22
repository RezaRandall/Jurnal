using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Handler.PaymentTerm;

public class PaymentTermInsertHandler : IRequestHandler<PaymentTermInsertDto, RowResponse<PaymentTermDto>>
{
    private readonly FinContext _context;
    public PaymentTermInsertHandler(FinContext db)
    {
        _context = db;
    }

    public class CommandValidator : AbstractValidator<PaymentTermInsertDto>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).MaximumLength(100);
            RuleFor(x => x.TermDays).NotNull().NotEmpty();
            RuleFor(x => x.IsActive).NotNull().NotEmpty();
        }
    }

    public async Task<RowResponse<PaymentTermDto>> Handle(PaymentTermInsertDto req, CancellationToken cancellationToken)
    {
        var result = new RowResponse<PaymentTermDto>();

        var paymentTerm = new Domain.PaymentTerms()
        {
            Name = req.Name
            ,Description = req.Description
            ,TermDays = req.TermDays
            ,IsActive = req.IsActive
        };

        try
        {
            await _context.PaymentTerm.AddAsync(paymentTerm, cancellationToken);
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
public class PaymentTermInsertDto : IRequest<RowResponse<PaymentTermDto>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TermDays { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
