namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodInsertHandler : IRequestHandler<PaymentMethodInsertDto, RowResponse<PaymentMethodDto>>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;

    public PaymentMethodInsertHandler(FinContext db, IPaymentMethodCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
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

            _cacheRemover.RemoveCache();

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


public class PaymentMethodInsertValidator : AbstractValidator<PaymentMethodInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public PaymentMethodInsertValidator(IUniqueNameValidationRepository repository)
    {
        _repository= repository;

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250)
            .MustAsync(
                async(model, name, cancellation) =>
                {
                    return await IsUniqueName(model, name, cancellation);
                }
            ).WithMessage("Name must be unique");

        RuleFor(x => x.Description).MaximumLength(250);
    }

    public async Task<bool> IsUniqueName(PaymentMethodInsertDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsPaymentMethodNameExist(name, 0);
        return !isExist;
    }
}
