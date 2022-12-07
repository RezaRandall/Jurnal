namespace TabpediaFin.Handler.ContactAddressHandler;

public class ContactAddressInsertHandler : IRequestHandler<ContactAddressInsertDto, RowResponse<ContactAddressFetchDto>>
{
    private readonly FinContext _context;
    private readonly IContactAddressCacheRemover _cacheRemover;

    public ContactAddressInsertHandler(FinContext db, IContactAddressCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ContactAddressFetchDto>> Handle(ContactAddressInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactAddressFetchDto>();

        var ContactAddress = new ContactAddress()
        {
            ContactId = request.ContactId,
            AddressName = request.AddressName,
            Address = request.Address,
            CityName = request.CityName,
            PostalCode = request.PostalCode,
            AddressTypeId = request.AddressTypeId,
            //AddresType = request.AddresType,
            Notes = request.Notes,
        };

        try
        {
            await _context.ContactAddress.AddAsync(ContactAddress, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _cacheRemover.RemoveCache();

            var row = new ContactAddressFetchDto()
            {
                Id = ContactAddress.Id,
                ContactId = ContactAddress.ContactId,
                AddressName = ContactAddress.AddressName,
                Address = ContactAddress.Address,
                CityName = ContactAddress.CityName,
                PostalCode = ContactAddress.PostalCode,
                AddressTypeId = ContactAddress.AddressTypeId,
                //AddresType = ContactAddress.AddresType,
                Notes = ContactAddress.Notes,
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



public class ContactAddressInsertDto : IRequest<RowResponse<ContactAddressFetchDto>>
{
    public int ContactId { get; set; } = 0;
    public string AddressName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public int AddressTypeId { get; set; } = 0;
    public string AddresType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

}

public class ContactAddressInsertValidator : AbstractValidator<ContactAddressInsertDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ContactAddressInsertValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.ContactId)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.AddressName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);
        RuleFor(x => x.Address)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250);
        RuleFor(x => x.CityName).MaximumLength(250);
        RuleFor(x => x.PostalCode).MaximumLength(250);
        RuleFor(x => x.AddressTypeId).NotNull().NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(250);
    }
}

