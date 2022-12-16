using TabpediaFin.Domain.TransferMoney;

namespace TabpediaFin.Handler.TransferMoneyHandler;

public class TransferMoneyUpdateHandler : IRequestHandler<TransferMoneyUpdateDto, RowResponse<TransferMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public TransferMoneyUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<TransferMoneyFetchDto>> Handle(TransferMoneyUpdateDto request, CancellationToken cancellationToken)
    {
        int transferMoneyId;
        var result = new RowResponse<TransferMoneyFetchDto>();
        List<TransferMoneyTag> TransferMoneyAttachment = new List<TransferMoneyTag>();
        List<TransferMoneyFetchAttachment> TransferMoneyFetchAttachment = new List<TransferMoneyFetchAttachment>();

        try
        {
            var tranferMoney = await _context.TransferMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            tranferMoney.TransferFromAccountId = request.TransferFromAccountId;
            tranferMoney.DepositToAccountId = request.DepositToAccountId;
            tranferMoney.Amount = request.Amount;
            tranferMoney.Memo = request.Memo;
            tranferMoney.TransactionNumber = request.TransactionNumber;
            tranferMoney.TransactionDate = request.TransactionDate;

            await _context.SaveChangesAsync(cancellationToken);
            transferMoneyId = request.Id;

            List<TransferMoneyFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, transferMoneyId, cancellationToken);
            List<TransferMoneyFetchTag> TagListResult = await UpdateTagAsync(request.TagList, transferMoneyId, cancellationToken);

            var row = new TransferMoneyFetchDto()
            {
                Id = request.Id,
                TransferFromAccountId = request.TransferFromAccountId,
                DepositToAccountId = request.DepositToAccountId,
                Amount = request.Amount,
                Memo = request.Memo,
                TransactionNumber = request.TransactionNumber,
                TransactionDate = request.TransactionDate,
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


    public async Task<List<TransferMoneyFetchAttachment>> UpdateAttachmentAsync(List<TransferMoneyAttachmentUpdate> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<TransferMoneyAttachment> TransferMoneyAttachmentList = new List<TransferMoneyAttachment>();
        List<TransferMoneyFetchAttachment> TransferMoneyFetchAttachmentList = new List<TransferMoneyFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (TransferMoneyAttachmentUpdate item in filedata)
            {
                TransferMoneyAttachmentList.Add(new TransferMoneyAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
                TransferMoneyFetchAttachmentList.Add(new TransferMoneyFetchAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
            }

            _context.TransferMoneyAttachment.UpdateRange(TransferMoneyAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return TransferMoneyFetchAttachmentList;
    }

    public async Task<List<TransferMoneyFetchTag>> UpdateTagAsync(List<TransferMoneyUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<TransferMoneyTag> TransferMoneyTag = new List<TransferMoneyTag>();
        List<TransferMoneyFetchTag> TransferMoneyFetchTag = new List<TransferMoneyFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (TransferMoneyUpdateTag item in filedata)
            {
                TransferMoneyTag.Add(new TransferMoneyTag
                {
                    Id = item.Id,
                    TagId = item.TagId,
                    TransId = TransId
                });
                TransferMoneyFetchTag.Add(new TransferMoneyFetchTag
                {
                    Id = item.Id,
                    TagId = item.TagId,
                    TransId = TransId
                });
            }
            _context.TransferMoneyTag.UpdateRange(TransferMoneyTag);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return TransferMoneyFetchTag;
    }



}

public class TransferMoneyUpdateDto : IRequest<RowResponse<TransferMoneyFetchDto>>
{
    public int Id { get; set; }
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public List<TransferMoneyAttachmentUpdate> AttachmentFile { get; set; }
    public List<TransferMoneyUpdateTag> TagList { get; set; }
}

public class TransferMoneyAttachmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class TransferMoneyUpdateTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}