namespace TabpediaFin.Handler.SalesOfferHandler
{
    public class SalesOfferUpdateHandler : IRequestHandler<SalesOfferUpdateDto, RowResponse<SalesOfferFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;
        public SalesOfferUpdateHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<SalesOfferFetchDto>> Handle(SalesOfferUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<SalesOfferFetchDto>();
            int transidresult;
            List<SalesOfferTag> SalesOfferAttachment = new List<SalesOfferTag>();
            List<SalesOfferFetchAttachment> SalesOfferFetchAttachment = new List<SalesOfferFetchAttachment>();
            try
            {
                var SalesOffer = await _context.SalesOffer.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                SalesOffer.StaffId = request.StaffId;
                SalesOffer.VendorId = request.VendorId;
                SalesOffer.TransDate = request.TransDate;
                SalesOffer.DueDate = request.DueDate;
                SalesOffer.TransCode = request.TransCode;
                SalesOffer.Status = request.Status;
                SalesOffer.Memo = request.Memo;
                SalesOffer.Notes = request.Notes;
                SalesOffer.DiscountType = request.DiscountType;
                SalesOffer.DiscountAmount = request.DiscountAmount;

                await _context.SaveChangesAsync(cancellationToken);
                transidresult = request.Id;

                List<SalesOfferFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<SalesOfferFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transidresult, cancellationToken);
                List<SalesOfferFetchItem> ItemListResult = await UpdateItemAsync(request.ItemList, transidresult, cancellationToken);

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
                    DiscountAmount = SalesOffer.DiscountAmount,
                    DiscountType = SalesOffer.DiscountType,
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

        public async Task<List<SalesOfferFetchAttachment>> UpdateAttachmentAsync(List<PurchestRequestAttahmentUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOfferAttachment> SalesOfferAttachmentList = new List<SalesOfferAttachment>();
            List<SalesOfferFetchAttachment> SalesOfferFetchAttachmentList = new List<SalesOfferFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentUpdateItem item in filedata)
                {
                    SalesOfferAttachmentList.Add(new SalesOfferAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    SalesOfferFetchAttachmentList.Add(new SalesOfferFetchAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                _context.SalesOfferAttachment.UpdateRange(SalesOfferAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOfferFetchAttachmentList;
        }
        public async Task<List<SalesOfferFetchTag>> UpdateTagAsync(List<SalesOfferUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOfferTag> SalesOfferTag = new List<SalesOfferTag>();
            List<SalesOfferFetchTag> SalesOfferFetchTag = new List<SalesOfferFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (SalesOfferUpdateTag item in filedata)
                {
                    SalesOfferTag.Add(new SalesOfferTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    SalesOfferFetchTag.Add(new SalesOfferFetchTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                _context.SalesOfferTag.UpdateRange(SalesOfferTag);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return SalesOfferFetchTag;
        }
        public async Task<List<SalesOfferFetchItem>> UpdateItemAsync(List<SalesOfferUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<SalesOfferItem> SalesOfferItem = new List<SalesOfferItem>();
            List<SalesOfferFetchItem> SalesOfferFetchItem = new List<SalesOfferFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (SalesOfferUpdateItem item in filedata)
                {
                    SalesOfferItem.Add(new SalesOfferItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    SalesOfferFetchItem.Add(new SalesOfferFetchItem
                    {
                        Id = item.Id,
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


    public class SalesOfferUpdateDto : IRequest<RowResponse<SalesOfferFetchDto>>
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
        public List<SalesOfferUpdateTag> TagList { get; set; }
        public List<SalesOfferUpdateItem> ItemList { get; set; }
    }

    public class SalesOfferUpdateTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }

    public class SalesOfferUpdateItem
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
