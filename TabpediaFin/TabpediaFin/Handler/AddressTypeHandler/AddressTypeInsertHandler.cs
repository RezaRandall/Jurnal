using TabpediaFin.Handler.AddressTypeHandler;

namespace TabpediaFin.Handler.AddressTypeHandler;

public class AddressTypeInsertHandler : IRequestHandler<AddressTypeInsertDto, RowResponse<AddressTypeFetchDto>>
{
    private readonly FinContext _context;

    public AddressTypeInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<AddressTypeFetchDto>> Handle(AddressTypeInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AddressTypeFetchDto>();

        var AddressType = new AddressType()
        {
            Name = request.Name,
            Description = request.Description,
        };

        try
        {
            await _context.AddressType.AddAsync(AddressType, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new AddressTypeFetchDto()
            {
                Id = AddressType.Id,
                Name = AddressType.Name,
                Description = AddressType.Description,
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



public class AddressTypeInsertDto : IRequest<RowResponse<AddressTypeFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}
