

using TabpediaFin.Domain.Expense;
using TabpediaFin.Domain.TransferMoney;
using TabpediaFin.Handler.ExpenseHandler;
using TabpediaFin.Handler.UploadAttachmentHandler;

namespace TabpediaFin.Handler.TransferMoneyHandler;

public class TransferMoneyInsertHandler : IRequestHandler<TransferMoneyInsertDto, RowResponse<TransferMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public TransferMoneyInsertHandler(FinContext db, IWebHostEnvironment environment, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<TransferMoneyFetchDto>> Handle(TransferMoneyInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<TransferMoneyFetchDto>();
        int transIdResult;
        DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);

        var transferMoney = new TransferMoney()
        {
            TransferFromAccountId = request.TransferFromAccountId,
            DepositToAccountId = request.DepositToAccountId,
            Amount = request.Amount,
            Memo = request.Memo,
            TransactionNumber = request.TransactionNumber,
            TransactionDate = TransDate,

        };

        try
        {
            await _context.TransferMoney.AddAsync(transferMoney, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            transIdResult = transferMoney.Id;
            UploadAttachmentService service = new UploadAttachmentService();
            List<uploadreturn> filedata = await service.UploadAttachmentAsync(request.AttachmentFile, _currentUser.TenantId, transIdResult);
            List<TransferMoneyFetchAttachment> returnfile = await PostAttachmentAsync(filedata, cancellationToken);
            List<TransferMoneyFetchTag> TagListResult = await PostTagAsync(request.TagList, transIdResult, cancellationToken);

            var row = new TransferMoneyFetchDto()
            {
                TransferFromAccountId = transferMoney.TransferFromAccountId,
                DepositToAccountId = transferMoney.DepositToAccountId,
                Amount = transferMoney.Amount,
                Memo = transferMoney.Memo,
                TransactionNumber = transferMoney.TransactionNumber,
                TransactionDate = transferMoney.TransactionDate,

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

    public async Task<List<TransferMoneyFetchAttachment>> PostAttachmentAsync(List<uploadreturn> filedata, CancellationToken cancellationToken)
    {
        List<TransferMoneyAttachment> TransferMoneyAttachmentList = new List<TransferMoneyAttachment>();
        List<TransferMoneyFetchAttachment> TransferMoneyFetchAttachmentList = new List<TransferMoneyFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (uploadreturn item in filedata)
            {
                TransferMoneyAttachmentList.Add(new TransferMoneyAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = item.TransId,
                });
                TransferMoneyFetchAttachmentList.Add(new TransferMoneyFetchAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = item.TransId,
                });
            }

            await _context.TransferMoneyAttachment.AddRangeAsync(TransferMoneyAttachmentList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return TransferMoneyFetchAttachmentList;
    }

    public async Task<List<TransferMoneyFetchTag>> PostTagAsync(List<TransferMoneyInsertTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<TransferMoneyTag> TransferMoneyTag = new List<TransferMoneyTag>();
        List<TransferMoneyFetchTag> TransferMoneyFetchTag = new List<TransferMoneyFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (TransferMoneyInsertTag item in filedata)
            {
                TransferMoneyTag.Add(new TransferMoneyTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
                TransferMoneyFetchTag.Add(new TransferMoneyFetchTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
            }

            await _context.TransferMoneyTag.AddRangeAsync(TransferMoneyTag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return TransferMoneyFetchTag;
    }



}

public class TransferMoneyInsertDto : IRequest<RowResponse<TransferMoneyFetchDto>>
{
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public int Memo { get; set; } = 0;
    //public int Tag { get; set; } = 0;
    public string TransactionNumber { get; set; } = string.Empty;
    //public string FileName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public ICollection<IFormFile> AttachmentFile { get; set; }
    public List<TransferMoneyInsertTag> TagList { get; set; }
}

public class TransferMoneyInsertTag
{
    public int TagId { get; set; } = 0;
}