namespace TabpediaFin.Handler.ExportImportContactHandler
{
    public class ContactImportHandler : IRequestHandler<ContactImportInsertListDto, PagedListResponse<ContactReadFileListDto>>
    {
        private readonly FinContext _context;

        public ContactImportHandler(FinContext db)
        {
            _context = db;
        }
        public async Task<PagedListResponse<ContactReadFileListDto>> Handle(ContactImportInsertListDto request, CancellationToken cancellationToken)
        {
            var result = new PagedListResponse<ContactReadFileListDto>();
            List<Contact> valueadd = new List<Contact>();
            List<ContactReadFileListDto> row = new List<ContactReadFileListDto>();
            try
            {
                if (request.contactList.Count > 0)
                {
                    foreach (ContactImportInsertDto item in request.contactList)
                    {
                        valueadd.Add(new Contact
                        {
                            Name = item.Name,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            Email = item.Email,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Website = item.Website,
                            Npwp = item.Npwp,
                            GroupId = item.GroupId,
                            IsCustomer = item.IsCustomer,
                            IsVendor = item.IsVendor,
                            IsEmployee = item.IsEmployee,
                            IsOther = item.IsOther,
                            Notes = item.Notes,
                        });
                        row.Add(new ContactReadFileListDto
                        {
                            Name = item.Name,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            Email = item.Email,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Website = item.Website,
                            Npwp = item.Npwp,
                            GroupId = item.GroupId,
                            IsCustomer = item.IsCustomer,
                            IsVendor = item.IsVendor,
                            IsEmployee = item.IsEmployee,
                            IsOther = item.IsOther,
                            Notes = item.Notes,
                        });
                    }
                }
                await _context.Contact.AddRangeAsync(valueadd, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                result.RecordCount = row.Count();

                result.IsOk = true;
                result.List = row;
                result.ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }

    public class ContactImportInsertDto
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

    }

    public class ContactImportInsertListDto : IRequest<PagedListResponse<ContactReadFileListDto>>
    {
        public List<ContactImportInsertDto> contactList { get; set; }
    }
}
