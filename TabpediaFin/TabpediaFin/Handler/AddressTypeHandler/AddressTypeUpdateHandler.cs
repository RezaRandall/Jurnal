namespace TabpediaFin.Handler.AddressTypeHandler;

public class AddressTypeUpdateHandler : IRequestHandler<AddressTypeUpdateDto, RowResponse<AddressTypeFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;
    public AddressTypeUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<AddressTypeFetchDto>> Handle(AddressTypeUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<AddressTypeFetchDto>();

        try
        {
            var AddressType = await _context.AddressType.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            AddressType.Name = request.Name;
            AddressType.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new AddressTypeFetchDto()
            {
                Id = request.Id,
                Name = AddressType.Name,
                Description = AddressType.Description,
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


public class AddressTypeUpdateDto : IRequest<RowResponse<AddressTypeFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
