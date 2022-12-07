using TabpediaFin.Handler.ContactAddressHandler;
using TabpediaFin.Handler.ContactPersonHandler;

namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactUpdateHandler : IRequestHandler<ContactUpdateDto, RowResponse<ContactFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ContactUpdateHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<ContactFetchDto>> Handle(ContactUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ContactFetchDto>();
            int contactidresult;
            List<ContactAddress> ContactAddress = new List<ContactAddress>();
            List<ContactPerson> ContactPerson = new List<ContactPerson>();
            List<ContactAddressFetchDto> ContactAddressFetchDto = new List<ContactAddressFetchDto>();
            List<ContactPersonFetchDto> ContactPersonFetchDto = new List<ContactPersonFetchDto>();
            try
            {
                var Contact = await _context.Contact.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                Contact.Name = request.Name;
                Contact.Address = request.Address;
                Contact.CityName = request.CityName;
                Contact.PostalCode = request.PostalCode;
                Contact.Email = request.Email;
                Contact.Phone = request.Phone;
                Contact.Fax = request.Fax;
                Contact.Website = request.Website;
                Contact.Npwp = request.Npwp;
                Contact.GroupId = request.GroupId;
                Contact.IsCustomer = request.IsCustomer;
                Contact.IsVendor = request.IsVendor;
                Contact.IsEmployee = request.IsEmployee;
                Contact.IsOther = request.IsOther;
                Contact.Notes = request.Notes;

                await _context.SaveChangesAsync(cancellationToken);
                contactidresult = request.Id;
                if (request.ContactAddressList.Count > 0)
                {
                    foreach (ContactAddressUpdateDto item in request.ContactAddressList)
                    {
                        ContactAddress.Add(new ContactAddress
                        {
                            Id = item.Id,
                            ContactId = contactidresult,
                            AddressName = item.AddressName,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            AddressTypeId = item.AddressTypeId,
                            //AddresType = item.AddresType,
                            Notes = item.Notes,
                            CreatedUid = _currentUser.UserId,
                        });
                        ContactAddressFetchDto.Add(new ContactAddressFetchDto
                        {
                            Id = item.Id,
                            ContactId = contactidresult,
                            AddressName = item.AddressName,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            AddressTypeId = item.AddressTypeId,
                            Notes = item.Notes,
                        });
                    }
                    _context.ContactAddress.UpdateRange(ContactAddress);
                }
                if (request.ContactPersonList.Count > 0)
                {
                    foreach (ContactPersonUpdateDto item in request.ContactPersonList)
                    {
                        ContactPerson.Add(new ContactPerson
                        {
                            Id = item.Id,
                            ContactId = contactidresult,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Others = item.Others,
                            Notes = item.Notes,
                            CreatedUid = _currentUser.UserId,
                        });
                        ContactPersonFetchDto.Add(new ContactPersonFetchDto
                        {
                            Id = item.Id,
                            ContactId = contactidresult,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Others = item.Others,
                            Notes = item.Notes,
                        });
                    }
                    _context.ContactPerson.UpdateRange(ContactPerson);
                }
                if (ContactPerson.Count > 0 || ContactAddress.Count > 0)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }

                var row = new ContactFetchDto()
                {
                    Id = request.Id,
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
    public class ContactUpdateDto : IRequest<RowResponse<ContactFetchDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Npwp { get; set; } = string.Empty;
        public string groupName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsOther { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<ContactAddressUpdateDto> ContactAddressList { get; set; }
        public List<ContactPersonUpdateDto> ContactPersonList { get; set; }

    }
}
