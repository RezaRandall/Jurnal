using TabpediaFin.Handler.ContactAddressHandler;
using TabpediaFin.Handler.ContactPersonHandler;

namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactUpdateHandler : IRequestHandler<ContactUpdateDto, RowResponse<ContactFetchDto>>
    {
        public Task<RowResponse<ContactFetchDto>> Handle(ContactUpdateDto request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
