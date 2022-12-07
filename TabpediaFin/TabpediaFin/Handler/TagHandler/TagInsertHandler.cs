namespace TabpediaFin.Handler.TagHandler;

public class TagInsertHandler : IRequestHandler<TagInsertDto, RowResponse<TagFetchDto>>
{
    private readonly FinContext _context;
    private readonly ITagCacheRemover _cacheRemover;

    public TagInsertHandler(FinContext db, ITagCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<TagFetchDto>> Handle(TagInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TagFetchDto>();

        var Tag = new Tag()
        {
            Name = request.Name,
            Description = request.Description,
        };

        try
        {
            await _context.Tag.AddAsync(Tag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            _cacheRemover.RemoveCache();
            
            var row = new TagFetchDto()
            {
                Id = Tag.Id,
                Name = Tag.Name,
                Description = Tag.Description,
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



public class TagInsertDto : IRequest<RowResponse<TagFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}
public class TagInsertValidator : AbstractValidator<TagInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public TagInsertValidator(IUniqueNameValidationRepository repository)
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

    public async Task<bool> IsUniqueName(TagInsertDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsTagNameExist(name, 0);
        return !isExist;
    }
}