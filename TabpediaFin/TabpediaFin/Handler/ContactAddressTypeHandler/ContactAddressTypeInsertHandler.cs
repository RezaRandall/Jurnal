namespace TabpediaFin.Handler.ContactAddressTypeHandler;

public class ContactAddressTypeInsertHandler : IRequestHandler<ContactAddressTypeInsertDto, RowResponse<ContactAddressTypeFetchDto>>
{
    private readonly FinContext _context;
    private readonly IContactAddressTypeCacheRemover _cacheRemover;

    public ContactAddressTypeInsertHandler(FinContext db, IContactAddressTypeCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
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

            _cacheRemover.RemoveCache();

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

public class ContactAddressTypeInsertValidator : AbstractValidator<ContactAddressTypeInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ContactAddressTypeInsertValidator(IUniqueNameValidationRepository repository)
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

    public async Task<bool> IsUniqueName(ContactAddressTypeInsertDto model, string name, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsContactAddressTypeNameExist(name, 0);
        return !isExist;
    }
}

