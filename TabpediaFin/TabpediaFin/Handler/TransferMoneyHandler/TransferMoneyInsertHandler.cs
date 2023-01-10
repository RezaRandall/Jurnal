using TabpediaFin.Domain.TransferMoney;

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
        int transIdResult ;
        Int64 transferAmount ;
        DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);

        var transferMoney = new TransferMoney()
        {
            TransferFromAccountId = request.TransferFromAccountId,
            DepositToAccountId = request.DepositToAccountId,
            Amount = request.Amount,
            Memo = request.Memo,
            TransactionNumber = request.TransactionNumber,
            TransactionDate = TransDate
        };

        try
        {
            await _context.TransferMoney.AddAsync(transferMoney, cancellationToken);

            transferAmount = transferMoney.Amount;

            var accountCashAndBankPostTransfer = await _context.Account.FirstAsync(x => x.Id == request.TransferFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var balanceTransfer = accountCashAndBankPostTransfer.Balance ;
            if (balanceTransfer < transferAmount)
            {
                result.IsOk = false;
                result.ErrorMessage = "Insufficient balance for this transaction, Failed!";
            }
            else 
            {
            // Perhitungan 
            var balanceValueTransfer = balanceTransfer - transferAmount;
            accountCashAndBankPostTransfer.Balance = balanceValueTransfer;
            await _context.SaveChangesAsync(cancellationToken);

            var accountCashAndBankGetDepo = await _context.Account.FirstAsync(x => x.Id == request.DepositToAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var totalBalanceDepo = accountCashAndBankGetDepo.Balance;
            var balanceValDepo = totalBalanceDepo + transferAmount;
            accountCashAndBankGetDepo.Balance = balanceValDepo;
            await _context.SaveChangesAsync(cancellationToken);

            transIdResult = transferMoney.Id;
            List<TransferMoneyFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transIdResult, cancellationToken);
            List<TransferMoneyFetchTag> TagListResult = await PostTagAsync(request.TagList, transIdResult, cancellationToken);

                var row = new TransferMoneyFetchDto()
                {
                    TransferFromAccountId = transferMoney.TransferFromAccountId,
                    DepositToAccountId = transferMoney.DepositToAccountId,
                    Amount = transferMoney.Amount,
                    Memo = transferMoney.Memo,
                    TransactionNumber = transferMoney.TransactionNumber,
                    TransactionDate = transferMoney.TransactionDate
                };
                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = row;
            }
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public async Task<List<TransferMoneyFetchAttachment>> PostAttachmentAsync(List<TransferMoneyAttahmentFiles> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<TransferMoneyAttachment> TransferMoneyAttachmentList = new List<TransferMoneyAttachment>();
        List<TransferMoneyFetchAttachment> TransferMoneyFetchAttachmentList = new List<TransferMoneyFetchAttachment>();

        if (filedata.Count > 0 )
        {
            foreach (TransferMoneyAttahmentFiles item in filedata)
            {
                TransferMoneyAttachmentList.Add(new TransferMoneyAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
                TransferMoneyFetchAttachmentList.Add(new TransferMoneyFetchAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
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

        if (filedata.Count > 0 )
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
    public Int64 Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public List<TransferMoneyAttahmentFiles> AttachmentFile { get; set; }
    public List<TransferMoneyInsertTag> TagList { get; set; }
}
public class TransferMoneyAttahmentFiles
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class TransferMoneyInsertTag
{
    public int TagId { get; set; } = 0;
}