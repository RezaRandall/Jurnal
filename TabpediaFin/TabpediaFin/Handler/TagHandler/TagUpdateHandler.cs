namespace TabpediaFin.Handler.TagHandler;

public class TagUpdateHandler : IRequestHandler<TagUpdateDto, RowResponse<TagFetchDto>>
{
    private readonly FinContext _context;

    public TagUpdateHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<TagFetchDto>> Handle(TagUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TagFetchDto>();

        try
        {
            var Tag = await _context.Tag.FirstAsync(x => x.Id == request.Id, cancellationToken);
            Tag.Name = request.Name;
            Tag.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new TagFetchDto()
            {
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
