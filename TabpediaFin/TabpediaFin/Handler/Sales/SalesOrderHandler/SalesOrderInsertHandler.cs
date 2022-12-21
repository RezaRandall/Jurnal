using TabpediaFin.Handler.UploadAttachmentHandler;
namespace TabpediaFin.Handler.SalesOrderHandler
{
    public class SalesOrderInsertHandler : IRequestHandler<SalesOrderInsertDto, RowResponse<SalesOrderFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public SalesOrderInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<SalesOrderFetchDto>> Handle(SalesOrderInsertDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesOrderFetchDto>();
            int transidresult;
            DateTime TransDate =  TimeZoneInfo.ConvertTimeToUtc(request.TransDate);
            DateTime DueDate = TimeZoneInfo.ConvertTimeToUtc(request.DueDate);
            var SalesOrder = new SalesOrder()
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
                await _context.SalesOrder.AddAsync(SalesOrder, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                transidresult = SalesOrder.Id;

                List<SalesOrderFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<SalesOrderFetchTag> TagListResult = await PostTagAsync(request.TagList, transidresult, cancellationToken);
                List<SalesOrderFetchItem> ItemListResult = await PostItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new SalesOrderFetchDto()
                {
                    Id = SalesOrder.Id,
                    StaffId = SalesOrder.StaffId,
                    VendorId = SalesOrder.VendorId,
                    WarehouseId = SalesOrder.WarehouseId,
                    TransDate = SalesOrder.TransDate,
                    DueDate = SalesOrder.DueDate,
                    TransCode = SalesOrder.TransCode,
                    Memo = SalesOrder.Memo,
                    Notes = SalesOrder.Notes,
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

        public async Task<List<SalesOrderFetchAttachment>> PostAttachmentAsync(List<PurchestRequestAttahmentItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOrderAttachment> SalesOrderAttachmentList = new List<SalesOrderAttachment>();
            List<SalesOrderFetchAttachment> SalesOrderFetchAttachmentList = new List<SalesOrderFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentItem item in filedata)
                {
                    SalesOrderAttachmentList.Add(new SalesOrderAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    SalesOrderFetchAttachmentList.Add(new SalesOrderFetchAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                await _context.SalesOrderAttachment.AddRangeAsync(SalesOrderAttachmentList, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOrderFetchAttachmentList;
        }
        public async Task<List<SalesOrderFetchTag>> PostTagAsync(List<SalesOrderInsertTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOrderTag> SalesOrderTag = new List<SalesOrderTag>();
            List<SalesOrderFetchTag> SalesOrderFetchTag = new List<SalesOrderFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (SalesOrderInsertTag item in filedata)
                {
                    SalesOrderTag.Add(new SalesOrderTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    SalesOrderFetchTag.Add(new SalesOrderFetchTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                await _context.SalesOrderTag.AddRangeAsync(SalesOrderTag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOrderFetchTag;
        }
        public async Task<List<SalesOrderFetchItem>> PostItemAsync(List<SalesOrderInsertItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOrderItem> SalesOrderItem = new List<SalesOrderItem>();
            List<SalesOrderFetchItem> SalesOrderFetchItem = new List<SalesOrderFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (SalesOrderInsertItem item in filedata)
                {
                    SalesOrderItem.Add(new SalesOrderItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId,
                        TaxId = item.TaxId

                    });
                    SalesOrderFetchItem.Add(new SalesOrderFetchItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId,
                        TaxId = item.TaxId
                    });
                }

                await _context.SalesOrderItem.AddRangeAsync(SalesOrderItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOrderFetchItem;
        }
    }

    public class SalesOrderInsertDto : IRequest<RowResponse<SalesOrderFetchDto>>
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
        public int DiscountType { get; set; }
        public double DiscountAmount { get; set; }
        public List<PurchestRequestAttahmentItem> AttachmentFile { get; set; }
        public List<SalesOrderInsertTag> TagList { get; set; }
        public List<SalesOrderInsertItem> ItemList { get; set; }
    }

    public class SalesOrderInsertTag
    {
        public int TagId { get; set; }
    }

    public class SalesOrderInsertItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitMeasureId { get; set; }
        public int TaxId { get; set; }
    }

    public class PurchestRequestAttahmentItem
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
    }
}
