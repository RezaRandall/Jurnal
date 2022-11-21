﻿using TabpediaFin.Handler.ContactAddressHandler;
using TabpediaFin.Handler.ContactPersonHandler;

namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactInsertHandler : IRequestHandler<ContactInsertDto, RowResponse<ContactFetchDto>>
    {
        public Task<RowResponse<ContactFetchDto>> Handle(ContactInsertDto request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
        public List<ContactAddressFetchDto> ContactAddressList { get; set; }
        public List<ContactPersonFetchDto> ContactPersonList { get; set; }

    }
}
