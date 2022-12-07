namespace TabpediaFin.Handler.TagHandler;

public class TagUpdateHandler : IRequestHandler<TagUpdateDto, RowResponse<TagFetchDto>>
{
    private readonly FinContext _context;
    private readonly ITagCacheRemover _cacheRemover;

    public TagUpdateHandler(FinContext db, ITagCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<TagFetchDto>> Handle(TagUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TagFetchDto>();

        try
        {
            var Tag = await _context.Tag.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (Tag == null || Tag.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            Tag.Name = request.Name;
            Tag.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            _cacheRemover.RemoveCache();

            var row = new TagFetchDto()
            {
                Id = request.Id,
                Name = Tag.Name,
                Description = Tag.Description,
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


public class TagUpdateDto : IRequest<RowResponse<TagFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
public class TagUpdateValidator : AbstractValidator<TagUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public TagUpdateValidator(IUniqueNameValidationRepository repository)
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

    public async Task<bool> IsUniqueName(TagUpdateDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsTagNameExist(name, model.Id);
        return !isExist;
    }
}