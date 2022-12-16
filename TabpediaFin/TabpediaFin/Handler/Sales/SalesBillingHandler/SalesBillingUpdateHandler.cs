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
            int transidresult;
            List<SalesBillingTag> SalesBillingAttachment = new List<SalesBillingTag>();
            List<SalesBillingFetchAttachment> SalesBillingFetchAttachment = new List<SalesBillingFetchAttachment>();
            try
            {
                var SalesBilling = await _context.SalesBilling.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                SalesBilling.StaffId = request.StaffId;
                SalesBilling.VendorId = request.VendorId;
                SalesBilling.TransDate = request.TransDate;
                SalesBilling.DueDate = request.DueDate;
                SalesBilling.TransCode = request.TransCode;
                SalesBilling.Status = request.Status;
                SalesBilling.Memo = request.Memo;
                SalesBilling.Notes = request.Notes;
                SalesBilling.DiscountType = request.DiscountType;
                SalesBilling.DiscountAmount = request.DiscountAmount;

                await _context.SaveChangesAsync(cancellationToken);
                transidresult = request.Id;

                List<SalesBillingFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<SalesBillingFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transidresult, cancellationToken);
                List<SalesBillingFetchItem> ItemListResult = await UpdateItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new SalesBillingFetchDto()
                {
                    Id = SalesBilling.Id,
                    StaffId = SalesBilling.StaffId,
                    VendorId = SalesBilling.VendorId,
                    TransDate = SalesBilling.TransDate,
                    DueDate = SalesBilling.DueDate,
                    TransCode = SalesBilling.TransCode,
                    Memo = SalesBilling.Memo,
                    Notes = SalesBilling.Notes,
                    DiscountAmount = SalesBilling.DiscountAmount,
                    DiscountType = SalesBilling.DiscountType,
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

        public async Task<List<SalesBillingFetchAttachment>> UpdateAttachmentAsync(List<PurchestRequestAttahmentUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesBillingAttachment> SalesBillingAttachmentList = new List<SalesBillingAttachment>();
            List<SalesBillingFetchAttachment> SalesBillingFetchAttachmentList = new List<SalesBillingFetchAttachment>();
            List<int> idupdateattachment = new List<int>();
            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentUpdateItem item in filedata)
                {
                    idupdateattachment.Add(item.Id);
                    SalesBillingAttachmentList.Add(new SalesBillingAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    SalesBillingFetchAttachmentList.Add(new SalesBillingFetchAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }
                List<SalesBillingAttachment> SalesBillingAttachmentListUpdate = _context.SalesBillingAttachment.Where<SalesBillingAttachment>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdateattachment.Contains(x.Id)).ToList();
                _context.SalesBillingAttachment.RemoveRange(SalesBillingAttachmentListUpdate);
                _context.SalesBillingAttachment.UpdateRange(SalesBillingAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesBillingFetchAttachmentList;
        }
        public async Task<List<SalesBillingFetchTag>> UpdateTagAsync(List<SalesBillingUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesBillingTag> SalesBillingTag = new List<SalesBillingTag>();
            List<SalesBillingFetchTag> SalesBillingFetchTag = new List<SalesBillingFetchTag>();
            List<int> idupdatetag = new List<int>();

            if (filedata.Count > 0)
            {
                foreach (SalesBillingUpdateTag item in filedata)
                {
                    idupdatetag.Add(item.Id);
                    SalesBillingTag.Add(new SalesBillingTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    SalesBillingFetchTag.Add(new SalesBillingFetchTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                List<SalesBillingTag> SalesBillingTagListUpdate = _context.SalesBillingTag.Where<SalesBillingTag>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdatetag.Contains(x.Id)).ToList();
                _context.SalesBillingTag.RemoveRange(SalesBillingTagListUpdate);
                _context.SalesBillingTag.UpdateRange(SalesBillingTag);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesBillingFetchTag;
        }
        public async Task<List<SalesBillingFetchItem>> UpdateItemAsync(List<SalesBillingUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesBillingItem> SalesBillingItem = new List<SalesBillingItem>();
            List<SalesBillingFetchItem> SalesBillingFetchItem = new List<SalesBillingFetchItem>();
            List<int> idupdateitem = new List<int>();

            if (filedata.Count > 0)
            {
                foreach (SalesBillingUpdateItem item in filedata)
                {
                    SalesBillingItem.Add(new SalesBillingItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    SalesBillingFetchItem.Add(new SalesBillingFetchItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }
                List<SalesBillingItem> SalesBillingItemListUpdate = _context.SalesBillingItem.Where<SalesBillingItem>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdateitem.Contains(x.Id)).ToList();
                _context.SalesBillingItem.RemoveRange(SalesBillingItemListUpdate);
                await _context.SalesBillingItem.AddRangeAsync(SalesBillingItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesBillingFetchItem;
        }
    }


    public class SalesBillingUpdateDto : IRequest<RowResponse<SalesBillingFetchDto>>
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
