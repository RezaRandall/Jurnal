using TabpediaFin.Handler.UploadAttachmentHandler;
namespace TabpediaFin.Handler.SalesOfferHandler
{
    public class SalesOfferInsertHandler : IRequestHandler<SalesOfferInsertDto, RowResponse<SalesOfferFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public SalesOfferInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<SalesOfferFetchDto>> Handle(SalesOfferInsertDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesOfferFetchDto>();
            int transidresult;
            DateTime TransDate =  TimeZoneInfo.ConvertTimeToUtc(request.TransDate);
            DateTime DueDate = TimeZoneInfo.ConvertTimeToUtc(request.DueDate);
            var SalesOffer = new SalesOffer()
            {
                StaffId = request.StaffId,
                VendorId = request.VendorId,
                TransDate = TransDate,
                DueDate = DueDate,
                TransCode = request.TransCode,
                Memo = request.Memo,
                Notes = request.Notes,
                Status = request.Status
            };

            try
            {
                await _context.SalesOffer.AddAsync(SalesOffer, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                transidresult = SalesOffer.Id;

                List<SalesOfferFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<SalesOfferFetchTag> TagListResult = await PostTagAsync(request.TagList, transidresult, cancellationToken);
                List<SalesOfferFetchItem> ItemListResult = await PostItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new SalesOfferFetchDto()
                {
                    Id = SalesOffer.Id,
                    StaffId = SalesOffer.StaffId,
                    VendorId = SalesOffer.VendorId,
                    TransDate = SalesOffer.TransDate,
                    DueDate = SalesOffer.DueDate,
                    TransCode = SalesOffer.TransCode,
                    Memo = SalesOffer.Memo,
                    Notes = SalesOffer.Notes,
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

        public async Task<List<SalesOfferFetchAttachment>> PostAttachmentAsync(List<PurchestRequestAttahmentItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOfferAttachment> SalesOfferAttachmentList = new List<SalesOfferAttachment>();
            List<SalesOfferFetchAttachment> SalesOfferFetchAttachmentList = new List<SalesOfferFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentItem item in filedata)
                {
                    SalesOfferAttachmentList.Add(new SalesOfferAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    SalesOfferFetchAttachmentList.Add(new SalesOfferFetchAttachment
                    {
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                await _context.SalesOfferAttachment.AddRangeAsync(SalesOfferAttachmentList, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOfferFetchAttachmentList;
        }
        public async Task<List<SalesOfferFetchTag>> PostTagAsync(List<SalesOfferInsertTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOfferTag> SalesOfferTag = new List<SalesOfferTag>();
            List<SalesOfferFetchTag> SalesOfferFetchTag = new List<SalesOfferFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (SalesOfferInsertTag item in filedata)
                {
                    SalesOfferTag.Add(new SalesOfferTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    SalesOfferFetchTag.Add(new SalesOfferFetchTag
                    {
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                await _context.SalesOfferTag.AddRangeAsync(SalesOfferTag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOfferFetchTag;
        }
        public async Task<List<SalesOfferFetchItem>> PostItemAsync(List<SalesOfferInsertItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOfferItem> SalesOfferItem = new List<SalesOfferItem>();
            List<SalesOfferFetchItem> SalesOfferFetchItem = new List<SalesOfferFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (SalesOfferInsertItem item in filedata)
                {
                    SalesOfferItem.Add(new SalesOfferItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    SalesOfferFetchItem.Add(new SalesOfferFetchItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }

                await _context.SalesOfferItem.AddRangeAsync(SalesOfferItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOfferFetchItem;
        }
    }

    public class SalesOfferInsertDto : IRequest<RowResponse<SalesOfferFetchDto>>
    {
        public int StaffId { get; set; }
        public int VendorId { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TransCode { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int DiscountType { get; set; }
        public double DiscountAmount { get; set; }
        public List<PurchestRequestAttahmentItem> AttachmentFile { get; set; }
        public List<SalesOfferInsertTag> TagList { get; set; }
        public List<SalesOfferInsertItem> ItemList { get; set; }
    }

    public class SalesOfferInsertTag
    {
        public int TagId { get; set; }
    }

    public class SalesOfferInsertItem
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
