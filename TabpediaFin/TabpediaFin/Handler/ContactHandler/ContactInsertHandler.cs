using TabpediaFin.Handler.ContactAddressHandler;
using TabpediaFin.Handler.ContactPersonHandler;

namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactInsertHandler : IRequestHandler<ContactInsertDto, RowResponse<ContactFetchDto>>
    {
        private readonly FinContext _context;

        public ContactInsertHandler(FinContext db)
        {
            _context = db;
        }

        public async Task<RowResponse<ContactFetchDto>> Handle(ContactInsertDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ContactFetchDto>();
            int contactidresult;
            var Contact = new Contact()
            {
                Name = request.Name,
                Address = request.Address,
                CityName = request.CityName,
                PostalCode = request.PostalCode,
                Email = request.Email,
                Phone = request.Phone,
                Fax = request.Fax,
                Website = request.Website,
                Npwp = request.Npwp,
                GroupId = request.GroupId,
                IsCustomer = request.IsCustomer,
                IsVendor = request.IsVendor,
                IsEmployee = request.IsEmployee,
                IsOther = request.IsOther,
                Notes = request.Notes,
            };
            List<ContactAddressFetchDto> ContactAddress = new List<ContactAddressFetchDto>();
            List<ContactPersonFetchDto> ContactPerson = new List<ContactPersonFetchDto>();

            try
            {
                await _context.Contact.AddAsync(Contact, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                contactidresult = Contact.Id;
                if(request.ContactAddressList.Count > 0)
                {
                    foreach (ContactAddressInsertDto item in request.ContactAddressList)
                    {
                        ContactAddress.Add(new ContactAddressFetchDto
                        {
                            ContactId = contactidresult,
                            AddressName = item.AddressName,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            AddressTypeId = item.AddressTypeId,
                            AddresType = item.AddresType,
                            Notes = item.Notes,
                        });
                    }
                }
                if (request.ContactPersonList.Count > 0)
                {
                    foreach (ContactPersonInsertDto item in request.ContactPersonList)
                    {
                        ContactPerson.Add(new ContactPersonFetchDto
                        {
                            ContactId = contactidresult,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Others = item.Others,
                            Notes = item.Notes,
                        });
                    }
                }
                var row = new ContactFetchDto()
                {
                    Id = Contact.Id,
                    Name = Contact.Name,
                    Address = Contact.Address,
                    CityName = Contact.CityName,
                    PostalCode = Contact.PostalCode,
                    Email = Contact.Email,
                    Phone = Contact.Phone,
                    Fax = Contact.Fax,
                    Website = Contact.Website,
                    Npwp = Contact.Npwp,
                    GroupId = Contact.GroupId,
                    IsCustomer = Contact.IsCustomer,
                    IsVendor = Contact.IsVendor,
                    IsEmployee = Contact.IsEmployee,
                    IsOther = Contact.IsOther,
                    Notes = Contact.Notes,
                    ContactAddressList = ContactAddress,
                    ContactPersonList = ContactPerson,
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
    public class ContactInsertDto : IRequest<RowResponse<ContactFetchDto>>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Npwp { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsOther { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<ContactAddressInsertDto> ContactAddressList { get; set; }
        public List<ContactPersonInsertDto> ContactPersonList { get; set; }

    }
}
