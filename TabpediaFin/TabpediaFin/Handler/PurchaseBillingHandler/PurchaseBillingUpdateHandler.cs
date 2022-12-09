namespace TabpediaFin.Handler.PurchaseBillingHandler
{
    public class PurchaseBillingUpdateHandler : IRequestHandler<PurchaseBillingUpdateDto, RowResponse<PurchaseBillingFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;
        public PurchaseBillingUpdateHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseBillingFetchDto>> Handle(PurchaseBillingUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseBillingFetchDto>();
            int PurchaseBillingidresult;
            List<PurchaseBillingTag> PurchaseBillingAttachment = new List<PurchaseBillingTag>();
            //List<ContactPerson> ContactPerson = new List<ContactPerson>();
            List<PurchaseBillingFetchAttachment> PurchaseBillingFetchAttachment = new List<PurchaseBillingFetchAttachment>();
            //List<ContactPersonFetchDto> ContactPersonFetchDto = new List<ContactPersonFetchDto>();
            try
            {
                var PurchaseBilling = await _context.PurchaseBilling.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                PurchaseBilling.StaffId = request.StaffId;
                PurchaseBilling.VendorId = request.VendorId;
                PurchaseBilling.TransDate = request.TransDate;
                PurchaseBilling.DueDate = request.DueDate;
                PurchaseBilling.TransCode = request.TransCode;
                PurchaseBilling.BudgetYear = request.BudgetYear;
                PurchaseBilling.UrgentLevel = request.UrgentLevel;
                PurchaseBilling.Memo = request.Memo;
                PurchaseBilling.Notes = request.Notes;

                await _context.SaveChangesAsync(cancellationToken);
                PurchaseBillingidresult = request.Id;
                //if (request.TagList.Count > 0)
                //{
                //    foreach (PurchaseBillingUpdateTag item in request.TagList)
                //    {
                //        ContactAddress.Add(new ContactAddress
                //        {
                //            Id = item.Id,
                //            ContactId = contactidresult,
                //            AddressName = item.AddressName,
                //            Address = item.Address,
                //            CityName = item.CityName,
                //            PostalCode = item.PostalCode,
                //            AddressTypeId = item.AddressTypeId,
                //            //AddresType = item.AddresType,
                //            Notes = item.Notes,
                //        });
                //        ContactAddressFetchDto.Add(new ContactAddressFetchDto
                //        {
                //            Id = item.Id,
                //            ContactId = contactidresult,
                //            AddressName = item.AddressName,
                //            Address = item.Address,
                //            CityName = item.CityName,
                //            PostalCode = item.PostalCode,
                //            AddressTypeId = item.AddressTypeId,
                //            Notes = item.Notes,
                //        });
                //    }
                //    _context.ContactAddress.UpdateRange(ContactAddress);
                //}
                //if (request.ContactPersonList.Count > 0)
                //{
                //    foreach (ContactPersonUpdateDto item in request.ContactPersonList)
                //    {
                //        ContactPerson.Add(new ContactPerson
                //        {
                //            Id = item.Id,
                //            ContactId = contactidresult,
                //            Name = item.Name,
                //            Email = item.Email,
                //            Phone = item.Phone,
                //            Fax = item.Fax,
                //            Others = item.Others,
                //            Notes = item.Notes,
                //        });
                //        ContactPersonFetchDto.Add(new ContactPersonFetchDto
                //        {
                //            Id = item.Id,
                //            ContactId = contactidresult,
                //            Name = item.Name,
                //            Email = item.Email,
                //            Phone = item.Phone,
                //            Fax = item.Fax,
                //            Others = item.Others,
                //            Notes = item.Notes,
                //        });
                //    }
                //    _context.ContactPerson.UpdateRange(ContactPerson);
                //}
                //if (ContactPerson.Count > 0 || ContactAddress.Count > 0)
                //{
                //    await _context.SaveChangesAsync(cancellationToken);
                //}

                //var row = new ContactFetchDto()
                //{
                //    Id = request.Id,
                //    Name = Contact.Name,
                //    Address = Contact.Address,
                //    CityName = Contact.CityName,
                //    PostalCode = Contact.PostalCode,
                //    Email = Contact.Email,
                //    Phone = Contact.Phone,
                //    Fax = Contact.Fax,
                //    Website = Contact.Website,
                //    Npwp = Contact.Npwp,
                //    GroupId = Contact.GroupId,
                //    IsCustomer = Contact.IsCustomer,
                //    IsVendor = Contact.IsVendor,
                //    IsEmployee = Contact.IsEmployee,
                //    IsOther = Contact.IsOther,
                //    Notes = Contact.Notes,
                //    ContactAddressList = ContactAddressFetchDto,
                //    ContactPersonList = ContactPersonFetchDto,
                //};

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                //result.Row = row;
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }

    public class PurchaseBillingUpdateDto : IRequest<RowResponse<PurchaseBillingFetchDto>>
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int VendorId { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TransCode { get; set; } = string.Empty;
        public string BudgetYear { get; set; } = string.Empty;
        public int UrgentLevel { get; set; }
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public ICollection<IFormFile> AttachmentFile { get; set; }
        public List<PurchaseBillingUpdateTag> TagList { get; set; }
        public List<PurchaseBillingUpdateItem> ItemList { get; set; }
    }

    public class PurchaseBillingUpdateTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }

    public class PurchaseBillingUpdateItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitMeasureId { get; set; }
    }

    public class PurchestRequestAttahmentUpdateItem
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public int TransId { get; set; }
    }
}
