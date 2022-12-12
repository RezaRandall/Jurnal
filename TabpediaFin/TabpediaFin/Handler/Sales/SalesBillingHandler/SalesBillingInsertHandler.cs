using TabpediaFin.Handler.UploadAttachmentHandler;
namespace TabpediaFin.Handler.SalesBillingHandler
{
    public class SalesBillingInsertHandler : IRequestHandler<SalesBillingInsertDto, RowResponse<SalesBillingFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public SalesBillingInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<SalesBillingFetchDto>> Handle(SalesBillingInsertDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesBillingFetchDto>();
            int transidresult;
            DateTime TransDate =  TimeZoneInfo.ConvertTimeToUtc(request.TransDate);
            DateTime DueDate = TimeZoneInfo.ConvertTimeToUtc(request.DueDate);
            var SalesBilling = new SalesBilling()
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
                await _context.SalesBilling.AddAsync(SalesBilling, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                transidresult = SalesBilling.Id;

                List<SalesBillingFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<SalesBillingFetchTag> TagListResult = await PostTagAsync(request.TagList, transidresult, cancellationToken);
                List<SalesBillingFetchItem> ItemListResult = await PostItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new SalesBillingFetchDto()
                {
                    Id = SalesBilling.Id,
                    StaffId = SalesBilling.StaffId,
                    VendorId = SalesBilling.VendorId,
                    WarehouseId = SalesBilling.WarehouseId,
                    TransDate = SalesBilling.TransDate,
                    DueDate = SalesBilling.DueDate,
                    TransCode = SalesBilling.TransCode,
                    Memo = SalesBilling.Memo,
                    Notes = SalesBilling.Notes,
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

        public async Task<List<SalesBillingFetchAttachment>> PostAttachmentAsync(List<PurchestRequestAttahmentItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesBillingAttachment> SalesBillingAttachmentList = new List<SalesBillingAttachment>();
            List<SalesBillingFetchAttachment> SalesBillingFetchAttachmentList = new List<SalesBillingFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentItem item in filedata)
                {
                    SalesBillingAttachmentList.Add(new SalesBillingAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    SalesBillingFetchAttachmentList.Add(new SalesBillingFetchAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                await _context.SalesBillingAttachment.AddRangeAsync(SalesBillingAttachmentList, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesBillingFetchAttachmentList;
        }
        public async Task<List<SalesBillingFetchTag>> PostTagAsync(List<SalesBillingInsertTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesBillingTag> SalesBillingTag = new List<SalesBillingTag>();
            List<SalesBillingFetchTag> SalesBillingFetchTag = new List<SalesBillingFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (SalesBillingInsertTag item in filedata)
                {
                    SalesBillingTag.Add(new SalesBillingTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    SalesBillingFetchTag.Add(new SalesBillingFetchTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                await _context.SalesBillingTag.AddRangeAsync(SalesBillingTag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesBillingFetchTag;
        }
        public async Task<List<SalesBillingFetchItem>> PostItemAsync(List<SalesBillingInsertItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesBillingItem> SalesBillingItem = new List<SalesBillingItem>();
            List<SalesBillingFetchItem> SalesBillingFetchItem = new List<SalesBillingFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (SalesBillingInsertItem item in filedata)
                {
                    SalesBillingItem.Add(new SalesBillingItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    SalesBillingFetchItem.Add(new SalesBillingFetchItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }

                await _context.SalesBillingItem.AddRangeAsync(SalesBillingItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesBillingFetchItem;
        }
    }

    public class SalesBillingInsertDto : IRequest<RowResponse<SalesBillingFetchDto>>
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
        public List<SalesBillingInsertTag> TagList { get; set; }
        public List<SalesBillingInsertItem> ItemList { get; set; }
    }

    public class SalesBillingInsertTag
    {
        public int TagId { get; set; }
    }

    public class SalesBillingInsertItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ItemUnitMeasureId { get; set; }
    }

    public class PurchestRequestAttahmentItem
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public int TransId { get; set; }
    }
}
