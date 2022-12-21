using TabpediaFin.Handler.UploadAttachmentHandler;
namespace TabpediaFin.Handler.PurchaseOrderHandler
{
    public class PurchaseOrderInsertHandler : IRequestHandler<PurchaseOrderInsertDto, RowResponse<PurchaseOrderFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public PurchaseOrderInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseOrderFetchDto>> Handle(PurchaseOrderInsertDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseOrderFetchDto>();
            int transidresult;
            DateTime TransDate =  TimeZoneInfo.ConvertTimeToUtc(request.TransDate);
            DateTime DueDate = TimeZoneInfo.ConvertTimeToUtc(request.DueDate);
            var PurchaseOrder = new PurchaseOrder()
            {
                StaffId = request.StaffId,
                VendorId = request.VendorId,
                WarehouseId = request.WarehouseId,
                TransDate = TransDate,
                DueDate = DueDate,
                TransCode = request.TransCode,
                Memo = request.Memo,
                Notes = request.Notes,
                Status = request.Status
            };

            try
            {
                await _context.PurchaseOrder.AddAsync(PurchaseOrder, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                transidresult = PurchaseOrder.Id;

                List<PurchaseOrderFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<PurchaseOrderFetchTag> TagListResult = await PostTagAsync(request.TagList, transidresult, cancellationToken);
                List<PurchaseOrderFetchItem> ItemListResult = await PostItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new PurchaseOrderFetchDto()
                {
                    Id = PurchaseOrder.Id,
                    StaffId = PurchaseOrder.StaffId,
                    VendorId = PurchaseOrder.VendorId,
                    WarehouseId = PurchaseOrder.WarehouseId,
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

        public async Task<List<PurchaseOrderFetchAttachment>> PostAttachmentAsync(List<PurchestRequestAttahmentItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOrderAttachment> PurchaseOrderAttachmentList = new List<PurchaseOrderAttachment>();
            List<PurchaseOrderFetchAttachment> PurchaseOrderFetchAttachmentList = new List<PurchaseOrderFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentItem item in filedata)
                {
                    PurchaseOrderAttachmentList.Add(new PurchaseOrderAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    PurchaseOrderFetchAttachmentList.Add(new PurchaseOrderFetchAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                await _context.PurchaseOrderAttachment.AddRangeAsync(PurchaseOrderAttachmentList, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOrderFetchAttachmentList;
        }
        public async Task<List<PurchaseOrderFetchTag>> PostTagAsync(List<PurchaseOrderInsertTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOrderTag> PurchaseOrderTag = new List<PurchaseOrderTag>();
            List<PurchaseOrderFetchTag> PurchaseOrderFetchTag = new List<PurchaseOrderFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseOrderInsertTag item in filedata)
                {
                    PurchaseOrderTag.Add(new PurchaseOrderTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    PurchaseOrderFetchTag.Add(new PurchaseOrderFetchTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                await _context.PurchaseOrderTag.AddRangeAsync(PurchaseOrderTag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOrderFetchTag;
        }
        public async Task<List<PurchaseOrderFetchItem>> PostItemAsync(List<PurchaseOrderInsertItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOrderItem> PurchaseOrderItem = new List<PurchaseOrderItem>();
            List<PurchaseOrderFetchItem> PurchaseOrderFetchItem = new List<PurchaseOrderFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseOrderInsertItem item in filedata)
                {
                    PurchaseOrderItem.Add(new PurchaseOrderItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId,
                        TaxId = item.TaxId
                    });
                    PurchaseOrderFetchItem.Add(new PurchaseOrderFetchItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId,
                        TaxId = item.TaxId
                    });
                }

                await _context.PurchaseOrderItem.AddRangeAsync(PurchaseOrderItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOrderFetchItem;
        }
    }

    public class PurchaseOrderInsertDto : IRequest<RowResponse<PurchaseOrderFetchDto>>
    {
        public int StaffId { get; set; }
        public int VendorId { get; set; }
        public int WarehouseId { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TransCode { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public List<PurchestRequestAttahmentItem> AttachmentFile { get; set; }
        public List<PurchaseOrderInsertTag> TagList { get; set; }
        public List<PurchaseOrderInsertItem> ItemList { get; set; }
    }

    public class PurchaseOrderInsertTag
    {
        public int TagId { get; set; }
    }

    public class PurchaseOrderInsertItem
    {
        public int ItemId { get; set; }
        public int TaxId { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitMeasureId { get; set; }
    }

    public class PurchestRequestAttahmentItem
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
    }
}
