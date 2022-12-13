namespace TabpediaFin.Handler.PurchaseRequestHandler
{
    public class PurchaseRequestUpdateHandler : IRequestHandler<PurchaseRequestUpdateDto, RowResponse<PurchaseRequestFetchDto>>
    {
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;
        public PurchaseRequestUpdateHandler(FinContext db, ICurrentUser currentUser)
        {
            _context = db;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseRequestFetchDto>> Handle(PurchaseRequestUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<PurchaseRequestFetchDto>();
            int transidresult;
            List<PurchaseRequestTag> PurchaseRequestAttachment = new List<PurchaseRequestTag>();
            List<PurchaseRequestFetchAttachment> PurchaseRequestFetchAttachment = new List<PurchaseRequestFetchAttachment>();
            try
            {
                var PurchaseRequest = await _context.PurchaseRequest.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                PurchaseRequest.StaffId = request.StaffId;
                PurchaseRequest.VendorId = request.VendorId;
                PurchaseRequest.TransDate = request.TransDate;
                PurchaseRequest.DueDate = request.DueDate;
                PurchaseRequest.TransCode = request.TransCode;
                PurchaseRequest.BudgetYear = request.BudgetYear;
                PurchaseRequest.UrgentLevel = request.UrgentLevel;
                PurchaseRequest.Status = request.Status;
                PurchaseRequest.Memo = request.Memo;
                PurchaseRequest.Notes = request.Notes;

                await _context.SaveChangesAsync(cancellationToken);
                transidresult = request.Id;

                List<PurchaseRequestFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transidresult, cancellationToken);
                List<PurchaseRequestFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transidresult, cancellationToken);
                List<PurchaseRequestFetchItem> ItemListResult = await UpdateItemAsync(request.ItemList, transidresult, cancellationToken);

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

        public async Task<List<PurchaseRequestFetchAttachment>> UpdateAttachmentAsync(List<PurchestRequestAttahmentUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseRequestAttachment> PurchaseRequestAttachmentList = new List<PurchaseRequestAttachment>();
            List<PurchaseRequestFetchAttachment> PurchaseRequestFetchAttachmentList = new List<PurchaseRequestFetchAttachment>();

            if (filedata.Count > 0)
            {
                foreach (PurchestRequestAttahmentUpdateItem item in filedata)
                {
                    PurchaseRequestAttachmentList.Add(new PurchaseRequestAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                    PurchaseRequestFetchAttachmentList.Add(new PurchaseRequestFetchAttachment
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        FileUrl = item.FileUrl,
                        Extension = item.Extension,
                        FileSize = item.FileSize,
                        TransId = TransId,
                    });
                }

                _context.PurchaseRequestAttachment.UpdateRange(PurchaseRequestAttachmentList);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseRequestFetchAttachmentList;
        }
        public async Task<List<PurchaseRequestFetchTag>> UpdateTagAsync(List<PurchaseRequestUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseRequestTag> PurchaseRequestTag = new List<PurchaseRequestTag>();
            List<PurchaseRequestFetchTag> PurchaseRequestFetchTag = new List<PurchaseRequestFetchTag>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseRequestUpdateTag item in filedata)
                {
                    PurchaseRequestTag.Add(new PurchaseRequestTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                    PurchaseRequestFetchTag.Add(new PurchaseRequestFetchTag
                    {
                        Id = item.Id,
                        TagId = item.TagId,
                        TransId = TransId
                    });
                }

                _context.PurchaseRequestTag.UpdateRange(PurchaseRequestTag);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PurchaseRequestFetchTag;
        }
        public async Task<List<PurchaseRequestFetchItem>> UpdateItemAsync(List<PurchaseRequestUpdateItem> filedata, int TransId, CancellationToken cancellationToken)
        {
            List<PurchaseRequestItem> PurchaseRequestItem = new List<PurchaseRequestItem>();
            List<PurchaseRequestFetchItem> PurchaseRequestFetchItem = new List<PurchaseRequestFetchItem>();

            if (filedata.Count > 0)
            {
                foreach (PurchaseRequestUpdateItem item in filedata)
                {
                    PurchaseRequestItem.Add(new PurchaseRequestItem
                    {
                        Id = item.Id,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        ItemUnitMeasureId = item.ItemUnitMeasureId,
                        TransId = TransId

                    });
                    PurchaseRequestFetchItem.Add(new PurchaseRequestFetchItem
                    {
                        Id = item.Id,
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


    public class PurchaseRequestUpdateDto : IRequest<RowResponse<PurchaseRequestFetchDto>>
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
        public List<PurchaseRequestUpdateTag> TagList { get; set; }
        public List<PurchaseRequestUpdateItem> ItemList { get; set; }
    }

    public class PurchaseRequestUpdateTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
    }

    public class PurchaseRequestUpdateItem
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
