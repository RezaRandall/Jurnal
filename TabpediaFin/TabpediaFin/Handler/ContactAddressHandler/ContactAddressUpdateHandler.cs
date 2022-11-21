namespace TabpediaFin.Handler.ContactAddressHandler;

public class ContactAddressUpdateHandler : IRequestHandler<ContactAddressUpdateDto, RowResponse<ContactAddressFetchDto>>
{
    private readonly FinContext _context;

    public ContactAddressUpdateHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ContactAddressFetchDto>> Handle(ContactAddressUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactAddressFetchDto>();

        try
        {
            var ContactAddress = await _context.ContactAddress.FirstAsync(x => x.Id == request.Id, cancellationToken);
            ContactAddress.AddressName = request.AddressName;
            ContactAddress.Address = request.Address;
            ContactAddress.CityName = request.CityName;
            ContactAddress.PostalCode = request.PostalCode;
            ContactAddress.AddressTypeId = request.AddressTypeId;
            ContactAddress.AddresType = request.AddresType;
            ContactAddress.Notes = request.Notes;


            await _context.SaveChangesAsync(cancellationToken);

            var row = new ContactAddressFetchDto()
            {
                AddressName = ContactAddress.AddressName,
                Address = ContactAddress.Address,
                CityName = ContactAddress.CityName,
                PostalCode = ContactAddress.PostalCode,
                AddressTypeId = ContactAddress.AddressTypeId,
                AddresType = ContactAddress.AddresType,
                Notes = ContactAddress.Notes,
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


public class ContactAddressUpdateDto : IRequest<RowResponse<ContactAddressFetchDto>>
{
    public int Id { get; set; }
    public int ContactId { get; set; } = 0;
    public string AddressName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public int AddressTypeId { get; set; } = 0;
    public string AddresType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
