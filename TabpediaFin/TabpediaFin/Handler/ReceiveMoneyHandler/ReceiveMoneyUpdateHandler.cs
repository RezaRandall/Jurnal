using TabpediaFin.Domain.Expense;
using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Handler.ExpenseHandler;
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
        int transferMoneyId;
        var result = new RowResponse<ReceiveMoneyFetchDto>();
        List<ReceiveMoneyTag> ReceiveMoneyAttachment = new List<ReceiveMoneyTag>();
        List<ReceiveMoneyFetchAttachment> ReceiveMoneyFetchAttachment = new List<ReceiveMoneyFetchAttachment>();

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

            await _context.SaveChangesAsync(cancellationToken);
            transferMoneyId = request.Id;

            List<ReceiveMoneyFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transferMoneyId, cancellationToken);
            List<ReceiveMoneyFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transferMoneyId, cancellationToken);

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

    public async Task<List<ReceiveMoneyFetchAttachment>> UpdateAttachmentAsync(List<ReceiveMoneyAttachmentUpdate> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ReceiveMoneyAttachment> ReceiveMoneyAttachmentList = new List<ReceiveMoneyAttachment>();
        List<ReceiveMoneyFetchAttachment> ReceiveMoneyFetchAttachmentList = new List<ReceiveMoneyFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (ReceiveMoneyAttachmentUpdate item in filedata)
            {
                ReceiveMoneyAttachmentList.Add(new ReceiveMoneyAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
                ReceiveMoneyFetchAttachmentList.Add(new ReceiveMoneyFetchAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
            }

            _context.ReceiveMoneyAttachment.UpdateRange(ReceiveMoneyAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ReceiveMoneyFetchAttachmentList;
    }

    public async Task<List<ReceiveMoneyFetchTag>> UpdateTagAsync(List<ReceiveMoneyUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ReceiveMoneyTag> ReceiveMoneyTag = new List<ReceiveMoneyTag>();
        List<ReceiveMoneyFetchTag> ReceiveMoneyFetchTag = new List<ReceiveMoneyFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (ReceiveMoneyUpdateTag item in filedata)
            {
                ReceiveMoneyTag.Add(new ReceiveMoneyTag
                {
                    Id = item.Id,
                    TagId = item.TagId,
                    TransId = TransId
                });
                ReceiveMoneyFetchTag.Add(new ReceiveMoneyFetchTag
                {
                    Id = item.Id,
                    TagId = item.TagId,
                    TransId = TransId
                });
            }
            _context.ReceiveMoneyTag.UpdateRange(ReceiveMoneyTag);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return ReceiveMoneyFetchTag;
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
    public List<ReceiveMoneyAttachmentUpdate> AttachmentFile { get; set; }
    public List<ReceiveMoneyUpdateTag> TagList { get; set; }
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