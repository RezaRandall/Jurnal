using TabpediaFin.Handler.ContactAddressHandler;
using TabpediaFin.Handler.ContactPersonHandler;

namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactUpdateHandler : IRequestHandler<ContactUpdateDto, RowResponse<ContactFetchDto>>
    {
        private readonly FinContext _context;

        public ContactUpdateHandler(FinContext db)
        {
            _context = db;
        }
        public async Task<RowResponse<ContactFetchDto>> Handle(ContactUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ContactFetchDto>();
            int contactidresult;
            List<ContactAddressFetchDto> ContactAddress = new List<ContactAddressFetchDto>();
            List<ContactPersonFetchDto> ContactPerson = new List<ContactPersonFetchDto>();
            try
            {
                var Contact = await _context.Contact.FirstAsync(x => x.Id == request.Id, cancellationToken);
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

                }
                if (request.ContactPersonList.Count > 0)
                {
                    
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
        public List<ContactAddressFetchDto> ContactAddressList { get; set; }
        public List<ContactPersonFetchDto> ContactPersonList { get; set; }

    }
}
