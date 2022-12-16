namespace TabpediaFin.Handler.PurchaseOfferHandler
{
    public class PurchaseOfferUpdateHandler : IRequestHandler<PurchaseOfferUpdateDto, RowResponse<PurchaseOfferFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;
        public PurchaseOfferUpdateHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseOfferFetchDto>> Handle(PurchaseOfferUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseOfferFetchDto>();
            int transidresult;
            List<PurchaseOfferTag> PurchaseOfferAttachment = new List<PurchaseOfferTag>();
            List<PurchaseOfferFetchAttachment> PurchaseOfferFetchAttachment = new List<PurchaseOfferFetchAttachment>();
            try
            {
                var PurchaseOffer = await _context.PurchaseOffer.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                PurchaseOffer.StaffId = request.StaffId;
                PurchaseOffer.VendorId = request.VendorId;
                PurchaseOffer.TransDate = request.TransDate;
                PurchaseOffer.DueDate = request.DueDate;
                PurchaseOffer.TransCode = request.TransCode;
                PurchaseOffer.Status = request.Status;
                PurchaseOffer.Memo = request.Memo;
                PurchaseOffer.Notes = request.Notes;

                await _context.SaveChangesAsync(cancellationToken);
                transidresult = request.Id;

                List<PurchaseOfferFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<PurchaseOfferFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transidresult, cancellationToken);
                List<PurchaseOfferFetchItem> ItemListResult = await UpdateItemAsync(request.ItemList, transidresult, cancellationToken);

                var row = new PurchaseOfferFetchDto()
                {
                    Id = PurchaseOffer.Id,
                    StaffId = PurchaseOffer.StaffId,
                    VendorId = PurchaseOffer.VendorId,
                    TransDate = PurchaseOffer.TransDate,
                    DueDate = PurchaseOffer.DueDate,
                    TransCode = PurchaseOffer.TransCode,
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

        public async Task<List<PurchaseOfferFetchAttachment>> UpdateAttachmentAsync(List<PurchestRequestAttahmentUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOfferAttachment> PurchaseOfferAttachmentList = new List<PurchaseOfferAttachment>();
            List<PurchaseOfferFetchAttachment> PurchaseOfferFetchAttachmentList = new List<PurchaseOfferFetchAttachment>();
            List<int> idupdateattachment = new List<int>();
            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentUpdateItem item in filedata)
                {
                    idupdateattachment.Add(item.Id);
                    PurchaseOfferAttachmentList.Add(new PurchaseOfferAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    PurchaseOfferFetchAttachmentList.Add(new PurchaseOfferFetchAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }
                List<PurchaseOfferAttachment> PurchaseOfferAttachmentListUpdate = _context.PurchaseOfferAttachment.Where<PurchaseOfferAttachment>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdateattachment.Contains(x.Id)).ToList();
                _context.PurchaseOfferAttachment.RemoveRange(PurchaseOfferAttachmentListUpdate);
                _context.PurchaseOfferAttachment.UpdateRange(PurchaseOfferAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOfferFetchAttachmentList;
        }
        public async Task<List<PurchaseOfferFetchTag>> UpdateTagAsync(List<PurchaseOfferUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOfferTag> PurchaseOfferTag = new List<PurchaseOfferTag>();
            List<PurchaseOfferFetchTag> PurchaseOfferFetchTag = new List<PurchaseOfferFetchTag>();
            List<int> idupdatetag = new List<int>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseOfferUpdateTag item in filedata)
                {
                    idupdatetag.Add(item.Id);
                    PurchaseOfferTag.Add(new PurchaseOfferTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    PurchaseOfferFetchTag.Add(new PurchaseOfferFetchTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                List<PurchaseOfferTag> PurchaseOfferTagListUpdate = _context.PurchaseOfferTag.Where<PurchaseOfferTag>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdatetag.Contains(x.Id)).ToList();
                _context.PurchaseOfferTag.RemoveRange(PurchaseOfferTagListUpdate);
                _context.PurchaseOfferTag.UpdateRange(PurchaseOfferTag);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOfferFetchTag;
        }
        public async Task<List<PurchaseOfferFetchItem>> UpdateItemAsync(List<PurchaseOfferUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseOfferItem> PurchaseOfferItem = new List<PurchaseOfferItem>();
            List<PurchaseOfferFetchItem> PurchaseOfferFetchItem = new List<PurchaseOfferFetchItem>();
            List<int> idupdateitem = new List<int>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseOfferUpdateItem item in filedata)
                {
                    PurchaseOfferItem.Add(new PurchaseOfferItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    PurchaseOfferFetchItem.Add(new PurchaseOfferFetchItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId
                    });
                }
                List<PurchaseOfferItem> PurchaseOfferItemListUpdate = _context.PurchaseOfferItem.Where<PurchaseOfferItem>(x => x.TransId == TransId && x.TenantId == _currentUser.TenantId && !idupdateitem.Contains(x.Id)).ToList();
                _context.PurchaseOfferItem.RemoveRange(PurchaseOfferItemListUpdate);
                await _context.PurchaseOfferItem.AddRangeAsync(PurchaseOfferItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseOfferFetchItem;
        }
    }


    public class PurchaseOfferUpdateDto : IRequest<RowResponse<PurchaseOfferFetchDto>>
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
        public List<PurchaseOfferUpdateTag> TagList { get; set; }
        public List<PurchaseOfferUpdateItem> ItemList { get; set; }
    }

    public class PurchaseOfferUpdateTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }

    public class PurchaseOfferUpdateItem
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
