namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodUpdateHandler : IRequestHandler<PaymentMethodUpdateDto, RowResponse<PaymentMethodDto>>
{
    private readonly FinContext _context;
    private readonly IPaymentMethodCacheRemover _cacheRemover;

    public PaymentMethodUpdateHandler(FinContext db, IPaymentMethodCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<PaymentMethodDto>> Handle(PaymentMethodUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<PaymentMethodDto>();

        try
        {
            var paymentMethod = await _context.PaymentMethod.FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (paymentMethod == null || paymentMethod.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            paymentMethod.Name = request.Name;
            paymentMethod.Description = request.Description;
            paymentMethod.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _cacheRemover.RemoveCache();

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


public class PaymentMethodUpdateValidator : AbstractValidator<PaymentMethodUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public PaymentMethodUpdateValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250)
            .MustAsync(
                async (model, name, cancellation) =>
                {
                    return await IsUniqueName(model, name, cancellation);
                }
            ).WithMessage("Name must be unique");

        RuleFor(x => x.Description).MaximumLength(250);
    }

    public async Task<bool> IsUniqueName(PaymentMethodUpdateDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsPaymentMethodNameExist(name, model.Id);
        return !isExist;
    }
}
