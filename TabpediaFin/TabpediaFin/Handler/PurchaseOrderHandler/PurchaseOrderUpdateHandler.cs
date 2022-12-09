namespace TabpediaFin.Handler.PurchaseOrderHandler
{
    public class PurchaseOrderUpdateHandler : IRequestHandler<PurchaseOrderUpdateDto, RowResponse<PurchaseOrderFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;
        public PurchaseOrderUpdateHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseOrderFetchDto>> Handle(PurchaseOrderUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseOrderFetchDto>();
            int PurchaseOrderidresult;
            List<PurchaseOrderTag> PurchaseOrderAttachment = new List<PurchaseOrderTag>();
            //List<ContactPerson> ContactPerson = new List<ContactPerson>();
            List<PurchaseOrderFetchAttachment> PurchaseOrderFetchAttachment = new List<PurchaseOrderFetchAttachment>();
            //List<ContactPersonFetchDto> ContactPersonFetchDto = new List<ContactPersonFetchDto>();
            try
            {
                var PurchaseOrder = await _context.PurchaseOrder.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                PurchaseOrder.StaffId = request.StaffId;
                PurchaseOrder.VendorId = request.VendorId;
                PurchaseOrder.TransDate = request.TransDate;
                PurchaseOrder.DueDate = request.DueDate;
                PurchaseOrder.TransCode = request.TransCode;
                PurchaseOrder.BudgetYear = request.BudgetYear;
                PurchaseOrder.UrgentLevel = request.UrgentLevel;
                PurchaseOrder.Memo = request.Memo;
                PurchaseOrder.Notes = request.Notes;

                await _context.SaveChangesAsync(cancellationToken);
                PurchaseOrderidresult = request.Id;
                //if (request.TagList.Count > 0)
                //{
                //    foreach (PurchaseOrderUpdateTag item in request.TagList)
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

    public class PurchaseOrderUpdateDto : IRequest<RowResponse<PurchaseOrderFetchDto>>
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
        public List<PurchaseOrderUpdateTag> TagList { get; set; }
        public List<PurchaseOrderUpdateItem> ItemList { get; set; }
    }

    public class PurchaseOrderUpdateTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }

    public class PurchaseOrderUpdateItem
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
