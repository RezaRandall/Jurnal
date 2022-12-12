namespace TabpediaFin.Handler.SalesBillingHandler
{
    public class SalesBillingUpdateHandler : IRequestHandler<SalesBillingUpdateDto, RowResponse<SalesBillingFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;
        public SalesBillingUpdateHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<SalesBillingFetchDto>> Handle(SalesBillingUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesBillingFetchDto>();
            int SalesBillingidresult;
            List<SalesBillingTag> SalesBillingAttachment = new List<SalesBillingTag>();
            //List<ContactPerson> ContactPerson = new List<ContactPerson>();
            List<SalesBillingFetchAttachment> SalesBillingFetchAttachment = new List<SalesBillingFetchAttachment>();
            //List<ContactPersonFetchDto> ContactPersonFetchDto = new List<ContactPersonFetchDto>();
            try
            {
                var SalesBilling = await _context.SalesBilling.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                SalesBilling.StaffId = request.StaffId;
                SalesBilling.VendorId = request.VendorId;
                SalesBilling.WarehouseId = request.WarehouseId;
                SalesBilling.TransDate = request.TransDate;
                SalesBilling.DueDate = request.DueDate;
                SalesBilling.TransCode = request.TransCode;
                SalesBilling.Memo = request.Memo;
                SalesBilling.Notes = request.Notes;

                await _context.SaveChangesAsync(cancellationToken);
                SalesBillingidresult = request.Id;
                //if (request.TagList.Count > 0)
                //{
                //    foreach (SalesBillingUpdateTag item in request.TagList)
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

    public class SalesBillingUpdateDto : IRequest<RowResponse<SalesBillingFetchDto>>
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int VendorId { get; set; }
        public int WarehouseId { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TransCode { get; set; } = string.Empty;
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public ICollection<IFormFile> AttachmentFile { get; set; }
        public List<SalesBillingUpdateTag> TagList { get; set; }
        public List<SalesBillingUpdateItem> ItemList { get; set; }
    }

    public class SalesBillingUpdateTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }

    public class SalesBillingUpdateItem
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
