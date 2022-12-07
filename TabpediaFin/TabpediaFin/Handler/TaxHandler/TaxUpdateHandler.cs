namespace TabpediaFin.Handler.TaxHandler;

public class TaxUpdateHandler : IRequestHandler<TaxUpdateDto, RowResponse<TaxFetchDto>>
{
    private readonly FinContext _context;
    private readonly ITaxCacheRemover _cacheRemover;

    public TaxUpdateHandler(FinContext db, ITaxCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<TaxFetchDto>> Handle(TaxUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TaxFetchDto>();

        try
        {
            var Tax = await _context.Tax.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (Tax == null || Tax.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            Tax.Name = request.Name;
            Tax.Description = request.Description;
            Tax.RatePercent = request.RatePercent;

            await _context.SaveChangesAsync(cancellationToken);

            _cacheRemover.RemoveCache();

            var row = new TaxFetchDto()
            {
                Id = request.Id,
                Name = Tax.Name,
                Description = Tax.Description,
                RatePercent = Tax.RatePercent,
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


public class TaxUpdateDto : IRequest<RowResponse<TaxFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public double RatePercent { get; set; }
}

public class TaxUpdateValidator : AbstractValidator<TaxUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public TaxUpdateValidator(IUniqueNameValidationRepository repository)
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

    public async Task<bool> IsUniqueName(TaxUpdateDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsTaxNameExist(name, model.Id);
        return !isExist;
    }
}
