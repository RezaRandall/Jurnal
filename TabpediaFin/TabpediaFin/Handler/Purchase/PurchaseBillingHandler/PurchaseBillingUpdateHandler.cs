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
            int transidresult;
            List<PurchaseBillingTag> PurchaseBillingAttachment = new List<PurchaseBillingTag>();
            List<PurchaseBillingFetchAttachment> PurchaseBillingFetchAttachment = new List<PurchaseBillingFetchAttachment>();
            try
            {
                var PurchaseBilling = await _context.PurchaseBilling.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                PurchaseBilling.StaffId = request.StaffId;
                PurchaseBilling.VendorId = request.VendorId;
                PurchaseBilling.TransDate = request.TransDate;
                PurchaseBilling.DueDate = request.DueDate;
                PurchaseBilling.TransCode = request.TransCode;
                PurchaseBilling.Status = request.Status;
                PurchaseBilling.Memo = request.Memo;
                PurchaseBilling.Notes = request.Notes;
                PurchaseBilling.DiscountType = request.DiscountType;
                PurchaseBilling.DiscountAmount = request.DiscountAmount;

                await _context.SaveChangesAsync(cancellationToken);
                transidresult = request.Id;

                List<PurchaseBillingFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<PurchaseBillingFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transidresult, cancellationToken);
                List<PurchaseBillingFetchItem> ItemListResult = await UpdateItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new PurchaseBillingFetchDto()
                {
                    Id = PurchaseBilling.Id,
                    StaffId = PurchaseBilling.StaffId,
                    VendorId = PurchaseBilling.VendorId,
                    TransDate = PurchaseBilling.TransDate,
                    DueDate = PurchaseBilling.DueDate,
                    TransCode = PurchaseBilling.TransCode,
                    Memo = PurchaseBilling.Memo,
                    Notes = PurchaseBilling.Notes,
                    DiscountAmount = PurchaseBilling.DiscountAmount,
                    DiscountType = PurchaseBilling.DiscountType,
                    AttachmentList = returnfile,
                    TagList = TagListResult,
                    ItemList = ItemListResult
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

        public async Task<List<PurchaseBillingFetchAttachment>> UpdateAttachmentAsync(List<PurchestRequestAttahmentUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseBillingAttachment> PurchaseBillingAttachmentList = new List<PurchaseBillingAttachment>();
            List<PurchaseBillingFetchAttachment> PurchaseBillingFetchAttachmentList = new List<PurchaseBillingFetchAttachment>();
            List<int> idupdateattachment = new List<int>();
            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentUpdateItem item in filedata)
                {
                    idupdateattachment.Add(item.Id);
                    PurchaseBillingAttachmentList.Add(new PurchaseBillingAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    PurchaseBillingFetchAttachmentList.Add(new PurchaseBillingFetchAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }
                List<PurchaseBillingAttachment> PurchaseBillingAttachmentListUpdate = _context.PurchaseBillingAttachment.Where<PurchaseBillingAttachment>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdateattachment.Contains(x.Id)).ToList();
                _context.PurchaseBillingAttachment.RemoveRange(PurchaseBillingAttachmentListUpdate);
                _context.PurchaseBillingAttachment.UpdateRange(PurchaseBillingAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseBillingFetchAttachmentList;
        }
        public async Task<List<PurchaseBillingFetchTag>> UpdateTagAsync(List<PurchaseBillingUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseBillingTag> PurchaseBillingTag = new List<PurchaseBillingTag>();
            List<PurchaseBillingFetchTag> PurchaseBillingFetchTag = new List<PurchaseBillingFetchTag>();
            List<int> idupdatetag = new List<int>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseBillingUpdateTag item in filedata)
                {
                    idupdatetag.Add(item.Id);
                    PurchaseBillingTag.Add(new PurchaseBillingTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    PurchaseBillingFetchTag.Add(new PurchaseBillingFetchTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                List<PurchaseBillingTag> PurchaseBillingTagListUpdate = _context.PurchaseBillingTag.Where<PurchaseBillingTag>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdatetag.Contains(x.Id)).ToList();
                _context.PurchaseBillingTag.RemoveRange(PurchaseBillingTagListUpdate);
                _context.PurchaseBillingTag.UpdateRange(PurchaseBillingTag);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseBillingFetchTag;
        }
        public async Task<List<PurchaseBillingFetchItem>> UpdateItemAsync(List<PurchaseBillingUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseBillingItem> PurchaseBillingItem = new List<PurchaseBillingItem>();
            List<PurchaseBillingFetchItem> PurchaseBillingFetchItem = new List<PurchaseBillingFetchItem>();
            List<int> idupdateitem = new List<int>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseBillingUpdateItem item in filedata)
                {
                    PurchaseBillingItem.Add(new PurchaseBillingItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId,
                        TaxId = item.TaxId

                    });
                    PurchaseBillingFetchItem.Add(new PurchaseBillingFetchItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId,
                        TaxId = item.TaxId
                    });
                }
                List<PurchaseBillingItem> PurchaseBillingItemListUpdate = _context.PurchaseBillingItem.Where<PurchaseBillingItem>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdateitem.Contains(x.Id)).ToList();
                _context.PurchaseBillingItem.RemoveRange(PurchaseBillingItemListUpdate);
                await _context.PurchaseBillingItem.AddRangeAsync(PurchaseBillingItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseBillingFetchItem;
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
        public int Status { get; set; }
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int DiscountType { get; set; }
        public double DiscountAmount { get; set; }
        public List<PurchestRequestAttahmentUpdateItem> AttachmentFile { get; set; }
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
        public int TaxId { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitMeasureId { get; set; }
    }

    public class PurchestRequestAttahmentUpdateItem
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
    }
}
