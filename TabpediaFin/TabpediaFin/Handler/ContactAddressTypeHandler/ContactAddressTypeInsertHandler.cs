namespace TabpediaFin.Handler.ContactAddressTypeHandler;

public class ContactAddressTypeInsertHandler : IRequestHandler<ContactAddressTypeInsertDto, RowResponse<ContactAddressTypeFetchDto>>
{
    private readonly FinContext _context;

    public ContactAddressTypeInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ContactAddressTypeFetchDto>> Handle(ContactAddressTypeInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactAddressTypeFetchDto>();

        var AddressType = new ContactAddressType()
        {
            Name = request.Name,
            Description = request.Description,
        };

        try
        {
            await _context.ContactAddressType.AddAsync(AddressType, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ContactAddressTypeFetchDto()
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



public class ContactAddressTypeInsertDto : IRequest<RowResponse<ContactAddressTypeFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}
