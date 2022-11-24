namespace TabpediaFin.Handler.ContactAddressTypeHandler;

public class ContactAddressTypeUpdateHandler : IRequestHandler<ContactAddressTypeUpdateDto, RowResponse<ContactAddressTypeFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;
    public ContactAddressTypeUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ContactAddressTypeFetchDto>> Handle(ContactAddressTypeUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactAddressTypeFetchDto>();

        try
        {
            var AddressType = await _context.ContactAddressType.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            AddressType.Name = request.Name;
            AddressType.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ContactAddressTypeFetchDto()
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


public class ContactAddressTypeUpdateDto : IRequest<RowResponse<ContactAddressTypeFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
