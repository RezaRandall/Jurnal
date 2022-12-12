using TabpediaFin.Handler.UploadAttachmentHandler;
namespace TabpediaFin.Handler.PurchaseRequestHandler
{
    public class PurchaseRequestInsertHandler : IRequestHandler<PurchaseRequestInsertDto, RowResponse<PurchaseRequestFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public PurchaseRequestInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseRequestFetchDto>> Handle(PurchaseRequestInsertDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseRequestFetchDto>();
            int transidresult;
            DateTime TransDate =  TimeZoneInfo.ConvertTimeToUtc(request.TransDate);
            DateTime DueDate = TimeZoneInfo.ConvertTimeToUtc(request.DueDate);
            var PurchaseRequest = new PurchaseRequest()
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
                await _context.PurchaseRequest.AddAsync(PurchaseRequest, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                transidresult = PurchaseRequest.Id;

                List<PurchaseRequestFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<PurchaseRequestFetchTag> TagListResult = await PostTagAsync(request.TagList, transidresult, cancellationToken);
                List<PurchaseRequestFetchItem> ItemListResult = await PostItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new PurchaseRequestFetchDto()
                {
                    Id = PurchaseRequest.Id,
                    StaffId = PurchaseRequest.StaffId,
                    VendorId = PurchaseRequest.VendorId,
                    TransDate = PurchaseRequest.TransDate,
                    DueDate = PurchaseRequest.DueDate,
                    TransCode = PurchaseRequest.TransCode,
                    BudgetYear = PurchaseRequest.BudgetYear,
                    UrgentLevel = PurchaseRequest.UrgentLevel,
                    Memo = PurchaseRequest.Memo,
                    Notes = PurchaseRequest.Notes,
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

        public async Task<List<PurchaseRequestFetchAttachment>> PostAttachmentAsync(List<PurchestRequestAttahmentItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseRequestAttachment> PurchaseRequestAttachmentList = new List<PurchaseRequestAttachment>();
            List<PurchaseRequestFetchAttachment> PurchaseRequestFetchAttachmentList = new List<PurchaseRequestFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentItem item in filedata)
                {
                    PurchaseRequestAttachmentList.Add(new PurchaseRequestAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    PurchaseRequestFetchAttachmentList.Add(new PurchaseRequestFetchAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                await _context.PurchaseRequestAttachment.AddRangeAsync(PurchaseRequestAttachmentList, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseRequestFetchAttachmentList;
        }
        public async Task<List<PurchaseRequestFetchTag>> PostTagAsync(List<PurchaseRequestInsertTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseRequestTag> PurchaseRequestTag = new List<PurchaseRequestTag>();
            List<PurchaseRequestFetchTag> PurchaseRequestFetchTag = new List<PurchaseRequestFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseRequestInsertTag item in filedata)
                {
                    PurchaseRequestTag.Add(new PurchaseRequestTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    PurchaseRequestFetchTag.Add(new PurchaseRequestFetchTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                await _context.PurchaseRequestTag.AddRangeAsync(PurchaseRequestTag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseRequestFetchTag;
        }
        public async Task<List<PurchaseRequestFetchItem>> PostItemAsync(List<PurchaseRequestInsertItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseRequestItem> PurchaseRequestItem = new List<PurchaseRequestItem>();
            List<PurchaseRequestFetchItem> PurchaseRequestFetchItem = new List<PurchaseRequestFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseRequestInsertItem item in filedata)
                {
                    PurchaseRequestItem.Add(new PurchaseRequestItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    PurchaseRequestFetchItem.Add(new PurchaseRequestFetchItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }

                await _context.PurchaseRequestItem.AddRangeAsync(PurchaseRequestItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseRequestFetchItem;
        }
    }

    public class PurchaseRequestInsertDto : IRequest<RowResponse<PurchaseRequestFetchDto>>
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
        public List<PurchestRequestAttahmentItem> AttachmentFile { get; set; }
        public List<PurchaseRequestInsertTag> TagList { get; set; }
        public List<PurchaseRequestInsertItem> ItemList { get; set; }
    }

    public class PurchaseRequestInsertTag
    {
        public int TagId { get; set; }
    }

    public class PurchaseRequestInsertItem
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
