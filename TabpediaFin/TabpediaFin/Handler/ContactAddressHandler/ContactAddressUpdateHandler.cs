namespace TabpediaFin.Handler.ContactAddressHandler;

public class ContactAddressUpdateHandler : IRequestHandler<ContactAddressUpdateDto, RowResponse<ContactAddressFetchDto>>
{
    private readonly FinContext _context;
    private readonly IContactAddressCacheRemover _cacheRemover;

    public ContactAddressUpdateHandler(FinContext db, IContactAddressCacheRemover cacheRemover)
    {
        _context = db;
        _cacheRemover = cacheRemover;
    }

    public async Task<RowResponse<ContactAddressFetchDto>> Handle(ContactAddressUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ContactAddressFetchDto>();

        try
        {
            var ContactAddress = await _context.ContactAddress.FirstAsync(x => x.Id == request.Id, cancellationToken);

            if (ContactAddress == null || ContactAddress.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            ContactAddress.ContactId = request.ContactId;
            ContactAddress.AddressName = request.AddressName;
            ContactAddress.Address = request.Address;
            ContactAddress.CityName = request.CityName;
            ContactAddress.PostalCode = request.PostalCode;
            ContactAddress.AddressTypeId = request.AddressTypeId;
            //ContactAddress.AddresType = request.AddresType;
            ContactAddress.Notes = request.Notes;


            await _context.SaveChangesAsync(cancellationToken);
            
            _cacheRemover.RemoveCache();

            var row = new ContactAddressFetchDto()
            {
                Id = request.Id,
                ContactId = ContactAddress.ContactId,
                AddressName = ContactAddress.AddressName,
                Address = ContactAddress.Address,
                CityName = ContactAddress.CityName,
                PostalCode = ContactAddress.PostalCode,
                AddressTypeId = ContactAddress.AddressTypeId,
                //AddresType = ContactAddress.AddresType,
                Notes = ContactAddress.Notes,
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


public class ContactAddressUpdateDto : IRequest<RowResponse<ContactAddressFetchDto>>
{
    public int Id { get; set; }
    public int ContactId { get; set; } = 0;
    public string AddressName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public int AddressTypeId { get; set; } = 0;
    public string AddresType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class ContactAddressUpdateValidator : AbstractValidator<ContactAddressUpdateDto>
{
    private readonly IUniqueNameValidationRepository _repository;

    public ContactAddressUpdateValidator(IUniqueNameValidationRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEmpty();

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
