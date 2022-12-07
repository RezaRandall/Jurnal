namespace TabpediaFin.Handler.ContactAddressTypeHandler;

public class ContactAddressTypeUpdateHandler : IRequestHandler<ContactAddressTypeUpdateDto, RowResponse<ContactAddressTypeFetchDto>>
{
    private readonly FinContext _context;
    private readonly IContactAddressTypeCacheRemover _cacheRemover;

    public ContactAddressTypeUpdateHandler(FinContext db, IContactAddressTypeCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ContactAddressTypeFetchDto>> Handle(ContactAddressTypeUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactAddressTypeFetchDto>();

        try
        {
            var ContactAddressType = await _context.ContactAddressType.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (ContactAddressType == null || ContactAddressType.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }

            ContactAddressType.Name = request.Name;
            ContactAddressType.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
            
            _cacheRemover.RemoveCache();
            
            var row = new ContactAddressTypeFetchDto()
            {
                Id = request.Id,
                Name = ContactAddressType.Name,
                Description = ContactAddressType.Description,
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


public class ContactAddressTypeUpdateDto : IRequest<RowResponse<ContactAddressTypeFetchDto>>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}

public class ContactAddressTypeUpdateValidator : AbstractValidator<ContactAddressTypeUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ContactAddressTypeUpdateValidator(IUniqueNameValidationRepository repository)
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

    public async Task<bool> IsUniqueName(ContactAddressTypeUpdateDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsContactAddressTypeNameExist(name, model.Id);
        return !isExist;
    }
}