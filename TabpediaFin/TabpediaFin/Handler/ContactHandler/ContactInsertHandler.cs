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
            List<ContactAddress> ContactAddress = new List<ContactAddress>();
            List<ContactPerson> ContactPerson = new List<ContactPerson>();
            List<ContactAddressFetchDto> ContactAddressFetchDto = new List<ContactAddressFetchDto>();
            List<ContactPersonFetchDto> ContactPersonFetchDto = new List<ContactPersonFetchDto>();

            try
            {
                await _context.Contact.AddAsync(Contact, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                contactidresult = Contact.Id;
                if(request.ContactAddressList.Count > 0)
                {
                    foreach (ContactAddressInsert item in request.ContactAddressList)
                    {
                        ContactAddress.Add(new ContactAddress
                        {
                            ContactId = contactidresult,
                            AddressName = item.AddressName,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            AddressTypeId = item.AddressTypeId,
                            //AddresType = item.AddresType,
                            Notes = item.Notes,
                        });
                        ContactAddressFetchDto.Add(new ContactAddressFetchDto
                        {
                            ContactId = contactidresult,
                            AddressName = item.AddressName,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            AddressTypeId = item.AddressTypeId,
                            //AddresType = item.AddresType,
                            Notes = item.Notes,
                        });
                    }

                    await _context.ContactAddress.AddRangeAsync(ContactAddress, cancellationToken);
                }

                if (request.ContactPersonList.Count > 0)
                {
                    foreach (ContactPersonInsert item in request.ContactPersonList)
                    {
                        ContactPerson.Add(new ContactPerson
                        {
                            ContactId = contactidresult,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Others = item.Others,
                            Notes = item.Notes,
                        });
                        ContactPersonFetchDto.Add(new ContactPersonFetchDto
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
                    await _context.ContactPerson.AddRangeAsync(ContactPerson, cancellationToken);
                }

                if(ContactPerson.Count > 0 || ContactAddress.Count > 0)
                {
                    await _context.SaveChangesAsync(cancellationToken);
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
                    ContactAddressList = ContactAddressFetchDto,
                    ContactPersonList = ContactPersonFetchDto,
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
        public List<ContactAddressInsert> ContactAddressList { get; set; }
        public List<ContactPersonInsert> ContactPersonList { get; set; }

    }
    public class ContactAddressInsert
    {
        public string AddressName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int AddressTypeId { get; set; } = 0;
        public string AddresType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
    public class ContactPersonInsert
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Others { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
