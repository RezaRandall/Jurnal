using TabpediaFin.Domain.ReceiveMoney;
namespace TabpediaFin.Handler.ReceiveMoneyHandler;

public class ReceiveMoneyInsertHandler : IRequestHandler<ReceiveMoneyInsertDto, RowResponse<ReceiveMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ReceiveMoneyInsertHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ReceiveMoneyFetchDto>> Handle(ReceiveMoneyInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ReceiveMoneyFetchDto>();
        int transIdResult;
        Int64 receiveAmount;
        DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);

        var receiveMoney = new ReceiveMoney()
        {
            DepositToAccountId = request.DepositToAccountId,
            PayerId = request.PayerId,
            TransactionDate = TransDate,
            TransactionNo = request.TransactionNo,
            Memo = request.Memo,
            TotalAmount = request.TotalAmount,
            
        };
        var receiveMoneyList = new ReceiveMoneyList()
        {
            AccountId = request.ListOfReceiveMoney[0].AccountId
        };

        try
        {
            await _context.ReceiveMoney.AddAsync(receiveMoney, cancellationToken);

            receiveAmount = receiveMoney.TotalAmount;

            var account = await _context.Account.FirstAsync(x => x.Id == request.DepositToAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var accountBalance = account.Balance;
            var sumTotalBalanceAccount = receiveAmount + accountBalance;
            account.Balance = sumTotalBalanceAccount;

            var receiveFromAccountId = await _context.Account.FirstAsync(x => x.Id == request.ListOfReceiveMoney[0].AccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var receiveBalance = receiveFromAccountId.Balance;
            receiveFromAccountId.Balance = receiveBalance + sumTotalBalanceAccount;

           await _context.SaveChangesAsync(cancellationToken);



            transIdResult = receiveMoney.Id;

            List<ReceiveMoneyFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transIdResult, cancellationToken);
            List<ReceiveMoneyFetchTag> TagListResult = await PostTagAsync(request.TagList, transIdResult, cancellationToken);
            List<ReceiveMoneyFetchList> ReceiveMoneyListResult = await PostReceiveMoneyListAsync(request.ListOfReceiveMoney, transIdResult, cancellationToken);

            var row = new ReceiveMoneyFetchDto()
            {
                DepositToAccountId = receiveMoney.DepositToAccountId,
                PayerId = receiveMoney.PayerId,
                TransactionDate = receiveMoney.TransactionDate,
                TransactionNo = receiveMoney.TransactionNo,
                Memo = receiveMoney.Memo,
                TotalAmount = receiveMoney.TotalAmount,
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


    public async Task<List<ReceiveMoneyFetchAttachment>> PostAttachmentAsync(List<ReceiveMoneyAttachmentFiles> filedata, int transId, CancellationToken cancellationToken)
    {
        List<ReceiveMoneyAttachment> ReceiveMoneyAttachmentList = new List<ReceiveMoneyAttachment>();
        List<ReceiveMoneyFetchAttachment> ReceiveMoneyFetchAttachmentList = new List<ReceiveMoneyFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (ReceiveMoneyAttachmentFiles item in filedata)
            {
                ReceiveMoneyAttachmentList.Add(new ReceiveMoneyAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = transId,
                });
                ReceiveMoneyFetchAttachmentList.Add(new ReceiveMoneyFetchAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = transId,
                });
            }

            await _context.ReceiveMoneyAttachment.AddRangeAsync(ReceiveMoneyAttachmentList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ReceiveMoneyFetchAttachmentList;
    }

    public async Task<List<ReceiveMoneyFetchTag>> PostTagAsync(List<ReceiveMoneyInsertTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ReceiveMoneyTag> ReceiveMoneyTag = new List<ReceiveMoneyTag>();
        List<ReceiveMoneyFetchTag> ReceiveMoneyFetchTag = new List<ReceiveMoneyFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (ReceiveMoneyInsertTag item in filedata)
            {
                ReceiveMoneyTag.Add(new ReceiveMoneyTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
                ReceiveMoneyFetchTag.Add(new ReceiveMoneyFetchTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
            }

            await _context.ReceiveMoneyTag.AddRangeAsync(ReceiveMoneyTag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ReceiveMoneyFetchTag;
    }

    public async Task<List<ReceiveMoneyFetchList>> PostReceiveMoneyListAsync(List<ReceiveMoneyInsertList> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ReceiveMoneyList> ReceiveMoneyList = new List<ReceiveMoneyList>();
        List<ReceiveMoneyFetchList> ReceiveMoneyFetchList = new List<ReceiveMoneyFetchList>();

        if (filedata.Count > 0)
        {
            foreach (ReceiveMoneyInsertList item in filedata)
            {
                ReceiveMoneyList.Add(new ReceiveMoneyList
                {
                    PriceIncludesTax = item.PriceIncludesTax,
                    AccountId = item.AccountId,
                    Description = item.Description,
                    TaxId = item.TaxId,
                    Amount = item.Amount,
                    TransId = TransId
                });
                ReceiveMoneyFetchList.Add(new ReceiveMoneyFetchList
                {
                    PriceIncludesTax = item.PriceIncludesTax,
                    AccountId = item.AccountId,
                    Description = item.Description,
                    TaxId = item.TaxId,
                    Amount = item.Amount,
                    TransId = TransId
                });
            }

            await _context.ReceiveMoneyList.AddRangeAsync(ReceiveMoneyList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ReceiveMoneyFetchList;
        
    }



}

public class ReceiveMoneyInsertDto : IRequest<RowResponse<ReceiveMoneyFetchDto>>
{
    public int DepositToAccountId { get; set; } = 0;
    public int PayerId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = String.Empty;
    public string Memo { get; set; } = string.Empty;
    public Int64 TotalAmount { get; set; } = 0;
    public List<ReceiveMoneyAttachmentFiles> AttachmentFile { get; set; }
    public List<ReceiveMoneyInsertTag> TagList { get; set; }
    public List<ReceiveMoneyInsertList> ListOfReceiveMoney { get; set; }
}

public class ReceiveMoneyAttachmentFiles
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}
public class ReceiveMoneyInsertTag
{
    public int TagId { get; set; } = 0;
}

public class ReceiveMoneyInsertList
{
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
}