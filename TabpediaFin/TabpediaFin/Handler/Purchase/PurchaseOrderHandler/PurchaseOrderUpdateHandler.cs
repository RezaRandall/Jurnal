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
            int transidresult;
            List<PurchaseOrderTag> PurchaseOrderAttachment = new List<PurchaseOrderTag>();
            List<PurchaseOrderFetchAttachment> PurchaseOrderFetchAttachment = new List<PurchaseOrderFetchAttachment>();
            try
            {
                var PurchaseOrder = await _context.PurchaseOrder.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                PurchaseOrder.StaffId = request.StaffId;
                PurchaseOrder.VendorId = request.VendorId;
                PurchaseOrder.TransDate = request.TransDate;
                PurchaseOrder.DueDate = request.DueDate;
                PurchaseOrder.TransCode = request.TransCode;
                PurchaseOrder.Status = request.Status;
                PurchaseOrder.Memo = request.Memo;
                PurchaseOrder.Notes = request.Notes;

                await _context.SaveChangesAsync(cancellationToken);
                transidresult = request.Id;

                List<PurchaseOrderFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<PurchaseOrderFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transidresult, cancellationToken);
                List<PurchaseOrderFetchItem> ItemListResult = await UpdateItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new PurchaseOrderFetchDto()
                {
                    Id = PurchaseOrder.Id,
                    StaffId = PurchaseOrder.StaffId,
                    VendorId = PurchaseOrder.VendorId,
                    TransDate = PurchaseOrder.TransDate,
                    DueDate = PurchaseOrder.DueDate,
                    TransCode = PurchaseOrder.TransCode,
                    Memo = PurchaseOrder.Memo,
                    Notes = PurchaseOrder.Notes,
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

        public async Task<List<PurchaseOrderFetchAttachment>> UpdateAttachmentAsync(List<PurchestRequestAttahmentUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOrderAttachment> PurchaseOrderAttachmentList = new List<PurchaseOrderAttachment>();
            List<PurchaseOrderFetchAttachment> PurchaseOrderFetchAttachmentList = new List<PurchaseOrderFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentUpdateItem item in filedata)
                {
                    PurchaseOrderAttachmentList.Add(new PurchaseOrderAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    PurchaseOrderFetchAttachmentList.Add(new PurchaseOrderFetchAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                _context.PurchaseOrderAttachment.UpdateRange(PurchaseOrderAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOrderFetchAttachmentList;
        }
        public async Task<List<PurchaseOrderFetchTag>> UpdateTagAsync(List<PurchaseOrderUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOrderTag> PurchaseOrderTag = new List<PurchaseOrderTag>();
            List<PurchaseOrderFetchTag> PurchaseOrderFetchTag = new List<PurchaseOrderFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseOrderUpdateTag item in filedata)
                {
                    PurchaseOrderTag.Add(new PurchaseOrderTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    PurchaseOrderFetchTag.Add(new PurchaseOrderFetchTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                _context.PurchaseOrderTag.UpdateRange(PurchaseOrderTag);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOrderFetchTag;
        }
        public async Task<List<PurchaseOrderFetchItem>> UpdateItemAsync(List<PurchaseOrderUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOrderItem> PurchaseOrderItem = new List<PurchaseOrderItem>();
            List<PurchaseOrderFetchItem> PurchaseOrderFetchItem = new List<PurchaseOrderFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseOrderUpdateItem item in filedata)
                {
                    PurchaseOrderItem.Add(new PurchaseOrderItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    PurchaseOrderFetchItem.Add(new PurchaseOrderFetchItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }

                await _context.PurchaseOrderItem.AddRangeAsync(PurchaseOrderItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOrderFetchItem;
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
        public int Status { get; set; }
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public List<PurchestRequestAttahmentUpdateItem> AttachmentFile { get; set; }
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
