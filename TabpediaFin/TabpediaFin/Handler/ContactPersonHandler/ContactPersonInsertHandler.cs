﻿namespace TabpediaFin.Handler.ContactPersonHandler;

public class ContactPersonInsertHandler : IRequestHandler<ContactPersonInsertDto, RowResponse<ContactPersonFetchDto>>
{
    private readonly FinContext _context;
    private readonly IContactPersonCacheRemover _cacheRemover;

    public ContactPersonInsertHandler(FinContext db, IContactPersonCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ContactPersonFetchDto>> Handle(ContactPersonInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactPersonFetchDto>();

        var ContactPerson = new ContactPerson()
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Fax = request.Fax,
            Others = request.Others,
            Notes = request.Notes,
            ContactId = request.ContactId
        };

        try
        {
            await _context.ContactPerson.AddAsync(ContactPerson, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            _cacheRemover.RemoveCache();
            
            var row = new ContactPersonFetchDto()
            {
                ContactId = ContactPerson.ContactId,
                Id = ContactPerson.Id,
                Name = ContactPerson.Name,
                Email = ContactPerson.Email,
                Phone = ContactPerson.Phone,
                Fax = ContactPerson.Fax,
                Others = ContactPerson.Others,
                Notes = ContactPerson.Notes,
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



public class ContactPersonInsertDto : IRequest<RowResponse<ContactPersonFetchDto>>
{
    public int ContactId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Fax { get; set; } = string.Empty;
    public string Others { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

}

public class ContactPersonInsertValidator : AbstractValidator<ContactPersonInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ContactPersonInsertValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

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

