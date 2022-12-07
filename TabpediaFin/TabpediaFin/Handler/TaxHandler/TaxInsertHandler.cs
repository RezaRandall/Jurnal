using TabpediaFin.Handler.TaxHandler;

namespace TabpediaFin.Handler.TaxHandler;

public class TaxInsertHandler : IRequestHandler<TaxInsertDto, RowResponse<TaxFetchDto>>
{
    private readonly FinContext _context;
    private readonly ITaxCacheRemover _cacheRemover;

    public TaxInsertHandler(FinContext db, ITaxCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<TaxFetchDto>> Handle(TaxInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TaxFetchDto>();

        var Tax = new Tax()
        {
            Name = request.Name,
            Description = request.Description,
        };

        try
        {
            await _context.Tax.AddAsync(Tax, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            _cacheRemover.RemoveCache();
            
            var row = new TaxFetchDto()
            {
                Id = Tax.Id,
                Name = Tax.Name,
                Description = Tax.Description,
                RatePercent = Tax.RatePercent,
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



public class TaxInsertDto : IRequest<RowResponse<TaxFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public double RatePercent { get; set; }

}

public class TaxInsertValidator : AbstractValidator<TaxInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public TaxInsertValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

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

    public async Task<bool> IsUniqueName(TaxInsertDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsTaxNameExist(name, 0);
        return !isExist;
    }
}
