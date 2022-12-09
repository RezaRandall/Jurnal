using TabpediaFin.Handler.UploadAttachmentHandler;
namespace TabpediaFin.Handler.PurchaseBillingHandler
{
    public class PurchaseBillingInsertHandler : IRequestHandler<PurchaseBillingInsertDto, RowResponse<PurchaseBillingFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public PurchaseBillingInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseBillingFetchDto>> Handle(PurchaseBillingInsertDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseBillingFetchDto>();
            int transidresult;
            DateTime TransDate =  TimeZoneInfo.ConvertTimeToUtc(request.TransDate);
            DateTime DueDate = TimeZoneInfo.ConvertTimeToUtc(request.DueDate);
            var PurchaseBilling = new PurchaseBilling()
            {
                StaffId = request.StaffId,
                VendorId = request.VendorId,
                TransDate = TransDate,
                DueDate = DueDate,
                TransCode = request.TransCode,
                BudgetYear = request.BudgetYear,
                UrgentLevel = request.UrgentLevel,
                Memo = request.Memo,
                Notes = request.Notes,
                Status = request.Status
            };

            try
            {
                await _context.PurchaseBilling.AddAsync(PurchaseBilling, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                transidresult = PurchaseBilling.Id;

                List<PurchaseBillingFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<PurchaseBillingFetchTag> TagListResult = await PostTagAsync(request.TagList, transidresult, cancellationToken);
                List<PurchaseBillingFetchItem> ItemListResult = await PostItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new PurchaseBillingFetchDto()
                {
                    Id = PurchaseBilling.Id,
                    StaffId = PurchaseBilling.StaffId,
                    VendorId = PurchaseBilling.VendorId,
                    TransDate = PurchaseBilling.TransDate,
                    DueDate = PurchaseBilling.DueDate,
                    TransCode = PurchaseBilling.TransCode,
                    BudgetYear = PurchaseBilling.BudgetYear,
                    UrgentLevel = PurchaseBilling.UrgentLevel,
                    Memo = PurchaseBilling.Memo,
                    Notes = PurchaseBilling.Notes,
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

        public async Task<List<PurchaseBillingFetchAttachment>> PostAttachmentAsync(List<PurchestRequestAttahmentItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseBillingAttachment> PurchaseBillingAttachmentList = new List<PurchaseBillingAttachment>();
            List<PurchaseBillingFetchAttachment> PurchaseBillingFetchAttachmentList = new List<PurchaseBillingFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentItem item in filedata)
                {
                    PurchaseBillingAttachmentList.Add(new PurchaseBillingAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    PurchaseBillingFetchAttachmentList.Add(new PurchaseBillingFetchAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                await _context.PurchaseBillingAttachment.AddRangeAsync(PurchaseBillingAttachmentList, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseBillingFetchAttachmentList;
        }
        public async Task<List<PurchaseBillingFetchTag>> PostTagAsync(List<PurchaseBillingInsertTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseBillingTag> PurchaseBillingTag = new List<PurchaseBillingTag>();
            List<PurchaseBillingFetchTag> PurchaseBillingFetchTag = new List<PurchaseBillingFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseBillingInsertTag item in filedata)
                {
                    PurchaseBillingTag.Add(new PurchaseBillingTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    PurchaseBillingFetchTag.Add(new PurchaseBillingFetchTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                await _context.PurchaseBillingTag.AddRangeAsync(PurchaseBillingTag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseBillingFetchTag;
        }
        public async Task<List<PurchaseBillingFetchItem>> PostItemAsync(List<PurchaseBillingInsertItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseBillingItem> PurchaseBillingItem = new List<PurchaseBillingItem>();
            List<PurchaseBillingFetchItem> PurchaseBillingFetchItem = new List<PurchaseBillingFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseBillingInsertItem item in filedata)
                {
                    PurchaseBillingItem.Add(new PurchaseBillingItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    PurchaseBillingFetchItem.Add(new PurchaseBillingFetchItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }

                await _context.PurchaseBillingItem.AddRangeAsync(PurchaseBillingItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseBillingFetchItem;
        }
    }

    public class PurchaseBillingInsertDto : IRequest<RowResponse<PurchaseBillingFetchDto>>
    {
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
        public List<PurchestRequestAttahmentItem> AttachmentFile { get; set; }
        public List<PurchaseBillingInsertTag> TagList { get; set; }
        public List<PurchaseBillingInsertItem> ItemList { get; set; }
    }

    public class PurchaseBillingInsertTag
    {
        public int TagId { get; set; }
    }

    public class PurchaseBillingInsertItem
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
