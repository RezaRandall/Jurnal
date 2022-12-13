namespace TabpediaFin.Handler.SalesOrderHandler
{
    public class SalesOrderUpdateHandler : IRequestHandler<SalesOrderUpdateDto, RowResponse<SalesOrderFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;
        public SalesOrderUpdateHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<SalesOrderFetchDto>> Handle(SalesOrderUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesOrderFetchDto>();
            int transidresult;
            List<SalesOrderTag> SalesOrderAttachment = new List<SalesOrderTag>();
            List<SalesOrderFetchAttachment> SalesOrderFetchAttachment = new List<SalesOrderFetchAttachment>();
            try
            {
                var SalesOrder = await _context.SalesOrder.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                SalesOrder.StaffId = request.StaffId;
                SalesOrder.VendorId = request.VendorId;
                SalesOrder.TransDate = request.TransDate;
                SalesOrder.DueDate = request.DueDate;
                SalesOrder.TransCode = request.TransCode;
                SalesOrder.Status = request.Status;
                SalesOrder.Memo = request.Memo;
                SalesOrder.Notes = request.Notes;
                SalesOrder.DiscountType = request.DiscountType;
                SalesOrder.DiscountAmount = request.DiscountAmount;

                await _context.SaveChangesAsync(cancellationToken);
                transidresult = request.Id;

                List<SalesOrderFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<SalesOrderFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transidresult, cancellationToken);
                List<SalesOrderFetchItem> ItemListResult = await UpdateItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new SalesOrderFetchDto()
                {
                    Id = SalesOrder.Id,
                    StaffId = SalesOrder.StaffId,
                    VendorId = SalesOrder.VendorId,
                    TransDate = SalesOrder.TransDate,
                    DueDate = SalesOrder.DueDate,
                    TransCode = SalesOrder.TransCode,
                    Memo = SalesOrder.Memo,
                    Notes = SalesOrder.Notes,
                    DiscountAmount = SalesOrder.DiscountAmount,
                    DiscountType = SalesOrder.DiscountType,
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

        public async Task<List<SalesOrderFetchAttachment>> UpdateAttachmentAsync(List<PurchestRequestAttahmentUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOrderAttachment> SalesOrderAttachmentList = new List<SalesOrderAttachment>();
            List<SalesOrderFetchAttachment> SalesOrderFetchAttachmentList = new List<SalesOrderFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentUpdateItem item in filedata)
                {
                    SalesOrderAttachmentList.Add(new SalesOrderAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    SalesOrderFetchAttachmentList.Add(new SalesOrderFetchAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                _context.SalesOrderAttachment.UpdateRange(SalesOrderAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOrderFetchAttachmentList;
        }
        public async Task<List<SalesOrderFetchTag>> UpdateTagAsync(List<SalesOrderUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOrderTag> SalesOrderTag = new List<SalesOrderTag>();
            List<SalesOrderFetchTag> SalesOrderFetchTag = new List<SalesOrderFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (SalesOrderUpdateTag item in filedata)
                {
                    SalesOrderTag.Add(new SalesOrderTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    SalesOrderFetchTag.Add(new SalesOrderFetchTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                _context.SalesOrderTag.UpdateRange(SalesOrderTag);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOrderFetchTag;
        }
        public async Task<List<SalesOrderFetchItem>> UpdateItemAsync(List<SalesOrderUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOrderItem> SalesOrderItem = new List<SalesOrderItem>();
            List<SalesOrderFetchItem> SalesOrderFetchItem = new List<SalesOrderFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (SalesOrderUpdateItem item in filedata)
                {
                    SalesOrderItem.Add(new SalesOrderItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    SalesOrderFetchItem.Add(new SalesOrderFetchItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }

                await _context.SalesOrderItem.AddRangeAsync(SalesOrderItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOrderFetchItem;
        }
    }


    public class SalesOrderUpdateDto : IRequest<RowResponse<SalesOrderFetchDto>>
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
        public List<SalesOrderUpdateTag> TagList { get; set; }
        public List<SalesOrderUpdateItem> ItemList { get; set; }
    }

    public class SalesOrderUpdateTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }

    public class SalesOrderUpdateItem
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
