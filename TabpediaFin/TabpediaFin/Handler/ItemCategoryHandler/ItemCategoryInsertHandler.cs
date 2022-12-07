using TabpediaFin.Handler.ItemCategoryHandler;

namespace TabpediaFin.Handler.ItemCategoryHandler;

public class ItemCategoryInsertHandler : IRequestHandler<ItemCategoryInsertDto, RowResponse<ItemCategoryFetchDto>>
{
    private readonly FinContext _context;
    private readonly IItemCategoryCacheRemover _cacheRemover;

    public ItemCategoryInsertHandler(FinContext db, IItemCategoryCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ItemCategoryFetchDto>> Handle(ItemCategoryInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemCategoryFetchDto>();

        var ItemCategory = new ItemCategory()
        {
            Name = request.Name,
            Description = request.Description,
        };

        try
        {
            await _context.ItemCategory.AddAsync(ItemCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _cacheRemover.RemoveCache();

            var row = new ItemCategoryFetchDto()
            {
                Id = ItemCategory.Id,
                Name = ItemCategory.Name,
                Description = ItemCategory.Description,
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



public class ItemCategoryInsertDto : IRequest<RowResponse<ItemCategoryFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}

public class ItemCategoryInsertValidator : AbstractValidator<ItemCategoryInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ItemCategoryInsertValidator(IUniqueNameValidationRepository repository)
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

    public async Task<bool> IsUniqueName(ItemCategoryInsertDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsItemCategoryNameExist(name, 0);
        return !isExist;
    }
}
