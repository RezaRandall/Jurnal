using TabpediaFin.Handler.AddressTypeHandler;

namespace TabpediaFin.Handler.ContactGroupHandler;

public class ContactGroupInsertHandler : IRequestHandler<ContactGroupInsertDto, RowResponse<ContactGroupFetchDto>>
{
    private readonly FinContext _context;

    public ContactGroupInsertHandler(FinContext db)
    {
        _context = db;
    }

    public async Task<RowResponse<ContactGroupFetchDto>> Handle(ContactGroupInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactGroupFetchDto>();

        var ContactGroup = new ContactGroup()
        {
            Name = request.Name,
            Description = request.Description,
        };

        try
        {
            await _context.ContactGroup.AddAsync(ContactGroup, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ContactGroupFetchDto()
            {
                Id = ContactGroup.Id,
                Name = ContactGroup.Name,
                Description = ContactGroup.Description,
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



public class ContactGroupInsertDto : IRequest<RowResponse<ContactGroupFetchDto>>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}
