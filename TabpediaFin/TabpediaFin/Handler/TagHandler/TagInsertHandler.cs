using TabpediaFin.Handler.AddressTypeHandler;

namespace TabpediaFin.Handler.TagHandler;

public class TagInsertHandler : IRequestHandler<TagInsertDto, RowResponse<TagFetchDto>>
{
    private readonly FinContext _context;

    public TagInsertHandler(FinContext db)
    {
        _context = db;
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
