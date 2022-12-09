using TabpediaFin.Handler.UploadAttachmentHandler;
namespace TabpediaFin.Handler.PurchaseOfferHandler
{
    public class PurchaseOfferInsertHandler : IRequestHandler<PurchaseOfferInsertDto, RowResponse<PurchaseOfferFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public PurchaseOfferInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseOfferFetchDto>> Handle(PurchaseOfferInsertDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseOfferFetchDto>();
            int transidresult;
            DateTime TransDate =  TimeZoneInfo.ConvertTimeToUtc(request.TransDate);
            DateTime DueDate = TimeZoneInfo.ConvertTimeToUtc(request.DueDate);
            var PurchaseOffer = new PurchaseOffer()
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
                await _context.PurchaseOffer.AddAsync(PurchaseOffer, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                transidresult = PurchaseOffer.Id;

                List<PurchaseOfferFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<PurchaseOfferFetchTag> TagListResult = await PostTagAsync(request.TagList, transidresult, cancellationToken);
                List<PurchaseOfferFetchItem> ItemListResult = await PostItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new PurchaseOfferFetchDto()
                {
                    Id = PurchaseOffer.Id,
                    StaffId = PurchaseOffer.StaffId,
                    VendorId = PurchaseOffer.VendorId,
                    TransDate = PurchaseOffer.TransDate,
                    DueDate = PurchaseOffer.DueDate,
                    TransCode = PurchaseOffer.TransCode,
                    BudgetYear = PurchaseOffer.BudgetYear,
                    UrgentLevel = PurchaseOffer.UrgentLevel,
                    Memo = PurchaseOffer.Memo,
                    Notes = PurchaseOffer.Notes,
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

        public async Task<List<PurchaseOfferFetchAttachment>> PostAttachmentAsync(List<PurchestRequestAttahmentItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOfferAttachment> PurchaseOfferAttachmentList = new List<PurchaseOfferAttachment>();
            List<PurchaseOfferFetchAttachment> PurchaseOfferFetchAttachmentList = new List<PurchaseOfferFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentItem item in filedata)
                {
                    PurchaseOfferAttachmentList.Add(new PurchaseOfferAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    PurchaseOfferFetchAttachmentList.Add(new PurchaseOfferFetchAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                await _context.PurchaseOfferAttachment.AddRangeAsync(PurchaseOfferAttachmentList, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOfferFetchAttachmentList;
        }
        public async Task<List<PurchaseOfferFetchTag>> PostTagAsync(List<PurchaseOfferInsertTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOfferTag> PurchaseOfferTag = new List<PurchaseOfferTag>();
            List<PurchaseOfferFetchTag> PurchaseOfferFetchTag = new List<PurchaseOfferFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseOfferInsertTag item in filedata)
                {
                    PurchaseOfferTag.Add(new PurchaseOfferTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    PurchaseOfferFetchTag.Add(new PurchaseOfferFetchTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                await _context.PurchaseOfferTag.AddRangeAsync(PurchaseOfferTag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOfferFetchTag;
        }
        public async Task<List<PurchaseOfferFetchItem>> PostItemAsync(List<PurchaseOfferInsertItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOfferItem> PurchaseOfferItem = new List<PurchaseOfferItem>();
            List<PurchaseOfferFetchItem> PurchaseOfferFetchItem = new List<PurchaseOfferFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseOfferInsertItem item in filedata)
                {
                    PurchaseOfferItem.Add(new PurchaseOfferItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    PurchaseOfferFetchItem.Add(new PurchaseOfferFetchItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }

                await _context.PurchaseOfferItem.AddRangeAsync(PurchaseOfferItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOfferFetchItem;
        }
    }

    public class PurchaseOfferInsertDto : IRequest<RowResponse<PurchaseOfferFetchDto>>
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
        public List<PurchaseOfferInsertTag> TagList { get; set; }
        public List<PurchaseOfferInsertItem> ItemList { get; set; }
    }

    public class PurchaseOfferInsertTag
    {
        public int TagId { get; set; }
    }

    public class PurchaseOfferInsertItem
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
