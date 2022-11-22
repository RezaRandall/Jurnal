using TabpediaFin.Handler.AddressTypeHandler;

namespace TabpediaFin.Handler.ContactAddressHandler;

public class ContactAddressInsertHandler : IRequestHandler<ContactAddressInsertDto, RowResponse<ContactAddressFetchDto>>
{
    private readonly FinContext _context;

    public ContactAddressInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ContactAddressFetchDto>> Handle(ContactAddressInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactAddressFetchDto>();

        var ContactAddress = new ContactAddress()
        {
            ContactId = request.ContactId,
            AddressName = request.AddressName,
            Address = request.Address,
            CityName = request.CityName,
            PostalCode = request.PostalCode,
            AddressTypeId = request.AddressTypeId,
            AddresType = request.AddresType,
            Notes = request.Notes,
        };

        try
        {
            await _context.ContactAddress.AddAsync(ContactAddress, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ContactAddressFetchDto()
            {
                Id = ContactAddress.Id,
                ContactId = ContactAddress.ContactId,
                AddressName = ContactAddress.AddressName,
                Address = ContactAddress.Address,
                CityName = ContactAddress.CityName,
                PostalCode = ContactAddress.PostalCode,
                AddressTypeId = ContactAddress.AddressTypeId,
                AddresType = ContactAddress.AddresType,
                Notes = ContactAddress.Notes,
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



public class ContactAddressInsertDto : IRequest<RowResponse<ContactAddressFetchDto>>
{
    public int ContactId { get; set; } = 0;
    public string AddressName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public int AddressTypeId { get; set; } = 0;
    public string AddresType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

}
