using TabpediaFin.Handler.ContactGroupHandler;

namespace TabpediaFin.Handler.ContactGroupHandler;

public class ContactGroupInsertHandler : IRequestHandler<ContactGroupInsertDto, RowResponse<ContactGroupFetchDto>>
{
    private readonly FinContext _context;
    private readonly IContactGroupCacheRemover _cacheRemover;

    public ContactGroupInsertHandler(FinContext db, IContactGroupCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
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

            _cacheRemover.RemoveCache();

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

public class ContactGroupInsertValidator : AbstractValidator<ContactGroupInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ContactGroupInsertValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250)
            .MustAsync(
                async (model, name, cancellation) =>
                {
                    return await IsUniqueName(model, name, cancellation);
                }
            ).WithMessage("Name must be unique");

        RuleFor(x => x.Description).MaximumLength(250);
    }

    public async Task<bool> IsUniqueName(ContactGroupInsertDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsContactGroupNameExist(name, 0);
        return !isExist;
    }
}
