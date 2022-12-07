namespace TabpediaFin.Handler.ContactPersonHandler;

public class ContactPersonUpdateHandler : IRequestHandler<ContactPersonUpdateDto, RowResponse<ContactPersonFetchDto>>
{
    private readonly FinContext _context;
    private readonly IContactPersonCacheRemover _cacheRemover;

    public ContactPersonUpdateHandler(FinContext db, IContactPersonCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ContactPersonFetchDto>> Handle(ContactPersonUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactPersonFetchDto>();

        try
        {
            var ContactPerson = await _context.ContactPerson.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (ContactPerson == null || ContactPerson.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            ContactPerson.ContactId = request.ContactId;
            ContactPerson.Name = request.Name;
            ContactPerson.Email = request.Email;
            ContactPerson.Phone = request.Phone ;
            ContactPerson.Fax = request.Fax ;
            ContactPerson.Others = request.Others ;
            ContactPerson.Notes = request.Notes ;

            await _context.SaveChangesAsync(cancellationToken);
            
            _cacheRemover.RemoveCache();

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

public class ContactPersonUpdateValidator : AbstractValidator<ContactPersonUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ContactPersonUpdateValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.ContactId)
             .NotNull()
             .NotEmpty();
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);
        RuleFor(x => x.Email).MaximumLength(250);
        RuleFor(x => x.Phone).MaximumLength(250);
        RuleFor(x => x.Fax).MaximumLength(250);
        RuleFor(x => x.Others).MaximumLength(250);
        RuleFor(x => x.Notes).MaximumLength(250);
    }
}
