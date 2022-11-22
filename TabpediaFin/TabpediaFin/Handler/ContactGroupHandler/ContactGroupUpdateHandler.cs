namespace TabpediaFin.Handler.ContactGroupHandler;

public class ContactGroupUpdateHandler : IRequestHandler<ContactGroupUpdateDto, RowResponse<ContactGroupFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ContactGroupUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ContactGroupFetchDto>> Handle(ContactGroupUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactGroupFetchDto>();

        try
        {
            var ContactGroup = await _context.ContactGroup.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            ContactGroup.Name = request.Name;
            ContactGroup.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ContactGroupFetchDto()
            {
                Id = request.Id,
                Name = ContactGroup.Name,
                Description = ContactGroup.Description,
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


public class ContactGroupUpdateDto : IRequest<RowResponse<ContactGroupFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
