namespace TabpediaFin.Handler.ContactGroupHandler;

public class ContactGroupUpdateHandler : IRequestHandler<ContactGroupUpdateDto, RowResponse<ContactGroupFetchDto>>
{
    private readonly FinContext _context;
    private readonly IContactGroupCacheRemover _cacheRemover;

    public ContactGroupUpdateHandler(FinContext db, IContactGroupCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ContactGroupFetchDto>> Handle(ContactGroupUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactGroupFetchDto>();

        try
        {
            var ContactGroup = await _context.ContactGroup.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (ContactGroup == null || ContactGroup.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            ContactGroup.Name = request.Name;
            ContactGroup.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
            
            _cacheRemover.RemoveCache();

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

public class ContactGroupUpdateValidator : AbstractValidator<ContactGroupUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ContactGroupUpdateValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty();

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

    public async Task<bool> IsUniqueName(ContactGroupUpdateDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsContactGroupNameExist(name, model.Id);
        return !isExist;
    }
}