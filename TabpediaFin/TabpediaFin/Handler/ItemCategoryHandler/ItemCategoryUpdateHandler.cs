namespace TabpediaFin.Handler.ItemCategoryHandler;

public class ItemCategoryUpdateHandler : IRequestHandler<ItemCategoryUpdateDto, RowResponse<ItemCategoryFetchDto>>
{
    private readonly FinContext _context;
    private readonly IItemCategoryCacheRemover _cacheRemover;

    public ItemCategoryUpdateHandler(FinContext db, IItemCategoryCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ItemCategoryFetchDto>> Handle(ItemCategoryUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemCategoryFetchDto>();

        try
        {
            var ItemCategory = await _context.ItemCategory.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (ItemCategory == null || ItemCategory.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            ItemCategory.Name = request.Name;
            ItemCategory.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            _cacheRemover.RemoveCache();

            var row = new ItemCategoryFetchDto()
            {
                Id = request.Id,
                Name = ItemCategory.Name,
                Description = ItemCategory.Description,
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


public class ItemCategoryUpdateDto : IRequest<RowResponse<ItemCategoryFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}

public class ItemCategoryUpdateValidator : AbstractValidator<ItemCategoryUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ItemCategoryUpdateValidator(IUniqueNameValidationRepository repository)
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

    public async Task<bool> IsUniqueName(ItemCategoryUpdateDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsItemCategoryNameExist(name, model.Id);
        return !isExist;
    }
}
