using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Handler.ReceiveMoneyHandler;

namespace TabpediaFin.Handler.ReceiveMoneyHandler;

public class ReceiveMoneyUpdateHandler : IRequestHandler<ReceiveMoneyUpdateDto, RowResponse<ReceiveMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ReceiveMoneyUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ReceiveMoneyFetchDto>> Handle(ReceiveMoneyUpdateDto request, CancellationToken cancellationToken)
    {
        int receiveMoneyId;
        var result = new RowResponse<ReceiveMoneyFetchDto>();

        List<ReceiveMoneyTag> receiveMoneyTag = new List<ReceiveMoneyTag>();
        List<ReceiveMoneyAttachment> receiveMoneyAttachment = new List<ReceiveMoneyAttachment>();
        List<ReceiveMoneyFetchTag> receiveMoneyFetchTag = new List<ReceiveMoneyFetchTag>();
        List<ReceiveMoneyFetchAttachment> receiveMoneyFetchAttachment = new List<ReceiveMoneyFetchAttachment>();

        try
        {
            var receiveMoney = await _context.ReceiveMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            receiveMoney.DepositToAccountId = request.DepositToAccountId;
            receiveMoney.VendorId = request.VendorId;
            receiveMoney.TransactionDate = request.TransactionDate;
            receiveMoney.TransactionNo = request.TransactionNo;
            receiveMoney.PriceIncludesTax = request.PriceIncludesTax;
            receiveMoney.ReceiveFromAccountId = request.ReceiveFromAccountId;
            receiveMoney.Description = request.Description;
            receiveMoney.TaxId = request.TaxId;
            receiveMoney.Amount = request.Amount;
            receiveMoney.Memo = request.Memo;
            receiveMoney.TotalAmount = request.TotalAmount;


            receiveMoneyId = request.Id;
            List<int> idUpdateReceiveMoneyTag = new List<int>();
            List<int> idUpdateReceiveMoneyAttachment = new List<int>();

            if (request.ReceiveMoneyTagList.Count > 0)
            {
                foreach (ReceiveMoneyUpdateTag i in request.ReceiveMoneyTagList)
                {
                    idUpdateReceiveMoneyTag.Add(i.Id);
                    receiveMoneyTag.Add(new ReceiveMoneyTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = receiveMoneyId,
                        CreatedUid = _currentUser.UserId
                    });
                    receiveMoneyFetchTag.Add(new ReceiveMoneyFetchTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = receiveMoneyId
                    });
                }
                _context.ReceiveMoneyTag.UpdateRange(receiveMoneyTag);
            }

            if (request.ReceiveMoneyAttachmentFile.Count > 0)
            {
                foreach (ReceiveMoneyAttachmentUpdate i in request.ReceiveMoneyAttachmentFile)
                {
                    idUpdateReceiveMoneyAttachment.Add(i.Id);
                    receiveMoneyAttachment.Add(new ReceiveMoneyAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        CreatedUid = _currentUser.UserId,
                        TransId = receiveMoneyId
                    });
                    receiveMoneyFetchAttachment.Add(new ReceiveMoneyFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        TransId = receiveMoneyId
                    });
                }
                _context.ReceiveMoneyAttachment.UpdateRange(receiveMoneyAttachment);
            }

            List<ReceiveMoneyTag> receiveMoneyTagList = _context.ReceiveMoneyTag.Where<ReceiveMoneyTag>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateReceiveMoneyTag.Contains(x.Id)).ToList();
            List<ReceiveMoneyAttachment> receiveMoneyAttachmentList = _context.ReceiveMoneyAttachment.Where<ReceiveMoneyAttachment>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateReceiveMoneyAttachment.Contains(x.Id)).ToList();
            _context.ReceiveMoneyTag.RemoveRange(receiveMoneyTagList);
            _context.ReceiveMoneyAttachment.RemoveRange(receiveMoneyAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ReceiveMoneyFetchDto()
            {
                Id = request.Id,
                DepositToAccountId = request.DepositToAccountId,
                VendorId = request.VendorId,
                TransactionDate = request.TransactionDate,
                TransactionNo = request.TransactionNo,
                PriceIncludesTax = request.PriceIncludesTax,
                ReceiveFromAccountId = request.ReceiveFromAccountId,
                Description = request.Description,
                TaxId = request.TaxId,
                Amount = request.Amount,
                Memo = request.Memo,
                TotalAmount = request.TotalAmount,
                ReceiveMoneyTagList = receiveMoneyFetchTag,
                ReceiveMoneyAttachmentList = receiveMoneyFetchAttachment,
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


}

public class ReceiveMoneyUpdateDto : IRequest<RowResponse<ReceiveMoneyFetchDto>>
{
    public int Id { get; set; }
    public int DepositToAccountId { get; set; } = 0;
    public int VendorId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public bool PriceIncludesTax { get; set; } = false;
    public int ReceiveFromAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public List<ReceiveMoneyAttachmentUpdate> ReceiveMoneyAttachmentFile { get; set; }
    public List<ReceiveMoneyUpdateTag> ReceiveMoneyTagList { get; set; }
}

public class ReceiveMoneyAttachmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class ReceiveMoneyUpdateTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}