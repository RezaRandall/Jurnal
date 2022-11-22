namespace TabpediaFin.Handler.ContactPersonHandler;

public class ContactPersonUpdateHandler : IRequestHandler<ContactPersonUpdateDto, RowResponse<ContactPersonFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ContactPersonUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ContactPersonFetchDto>> Handle(ContactPersonUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactPersonFetchDto>();

        try
        {
            var ContactPerson = await _context.ContactPerson.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            ContactPerson.ContactId = request.ContactId;
            ContactPerson.Name = request.Name;
            ContactPerson.Email = request.Email;
            ContactPerson.Phone = request.Phone ;
            ContactPerson.Fax = request.Fax ;
            ContactPerson.Others = request.Others ;
            ContactPerson.Notes = request.Notes ;


            await _context.SaveChangesAsync(cancellationToken);

            var row = new ContactPersonFetchDto()
            {
                Id = request.Id,
                ContactId = ContactPerson.ContactId,
                Name = ContactPerson.Name,
                Email = ContactPerson.Email,
                Phone = ContactPerson.Phone,
                Fax = ContactPerson.Fax,
                Others = ContactPerson.Others,
                Notes = ContactPerson.Notes,
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


public class ContactPersonUpdateDto : IRequest<RowResponse<ContactPersonFetchDto>>
{
    public int Id { get; set; }
    public int ContactId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Fax { get; set; } = string.Empty;
    public string Others { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
