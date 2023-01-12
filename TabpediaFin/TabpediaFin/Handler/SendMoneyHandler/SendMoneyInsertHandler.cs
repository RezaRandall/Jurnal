using NPOI.SS.Formula.Functions;
using TabpediaFin.Domain;
using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Domain.SendMoney;

namespace TabpediaFin.Handler.SendMoneyHandler;

public class SendMoneyInsertHandler : IRequestHandler<SendMoneyInsertDto, RowResponse<SendMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public SendMoneyInsertHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<SendMoneyFetchDto>> Handle(SendMoneyInsertDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<SendMoneyFetchDto>();
        int transIdResult;
        Int64 RequestAmountTotal;        
        DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);

        var sendMoney = new SendMoney()
        {
            PayFromAccountId = request.PayFromAccountId,
            RecipientContactId = request.RecipientContactId,
            TransactionDate = TransDate,
            TransactionNo = request.TransactionNo,
            Memo = request.Memo,
            TotalAmount = request.TotalAmount,
            WitholdingAmount = request.WitholdingAmount,
            DiscountAmount = request.DiscountAmount,
            DiscountPercent = request.DiscountPercent,
            DiscountForAccountId = request.DiscountForAccountId,
        };

        try
        {
            await _context.SendMoney.AddAsync(sendMoney, cancellationToken);
            RequestAmountTotal = sendMoney.TotalAmount;

            // BALANCE REDUCTION FROM PAYEER ACCOUNT
            var saldoPayeer = await _context.Account.FirstAsync(x => x.Id == request.PayFromAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
            var senderBalance = saldoPayeer.Balance;
            var sumReductionAmt = senderBalance - RequestAmountTotal;
            saldoPayeer.Balance = sumReductionAmt;

            // WHEN INCLUDE WITHOLDING
            if (request.DiscountAmount != 0 || request.DiscountPercent != 0 && request.DiscountForAccountId != 0)
            {
                var discountInput = await _context.Account.FirstAsync(x => x.Id == request.DiscountForAccountId && x.TenantId == _currentUser.TenantId, cancellationToken);
                var balanceAmount = discountInput.Balance;
                var witholding = request.WitholdingAmount;
                var countWitholding = balanceAmount + witholding;
                discountInput.Balance = countWitholding;
            }

            // BALANCE REDUCTION FROM RECIPIENT ACCOUNT
            foreach (SendMoneyInsertList i in request.SendMoneyList)
            {
                var paymentForAccountId = await _context.Account.FirstAsync(x => x.Id == i.AccountId && x.TenantId == _currentUser.TenantId, cancellationToken) ;
                var balanceAccountReceiver = paymentForAccountId.Balance;
                var sumBalance = balanceAccountReceiver - i.Amount;
                paymentForAccountId.Balance = sumBalance;
            }

            await _context.SaveChangesAsync(cancellationToken);
            transIdResult = sendMoney.Id;

            List<SendMoneyFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transIdResult, cancellationToken);
            List<SendMoneyFetchTag> TagListResult = await PostTagAsync(request.TagList, transIdResult, cancellationToken);
            List<SendMoneyFetchList> SendMoneyListResult = await PostSendMoneyListAsync(request.SendMoneyList, transIdResult, cancellationToken);

            var row = new SendMoneyFetchDto()
            {
                PayFromAccountId = sendMoney.PayFromAccountId,
                RecipientContactId = sendMoney.RecipientContactId,
                TransactionDate = sendMoney.TransactionDate,
                TransactionNo = sendMoney.TransactionNo,
                Memo = sendMoney.Memo,
                TotalAmount = sendMoney.TotalAmount,
                WitholdingAmount = sendMoney.WitholdingAmount,
                DiscountAmount = sendMoney.DiscountAmount,
                DiscountPercent = sendMoney.DiscountPercent,
                DiscountForAccountId = request.DiscountForAccountId,
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

    public async Task<List<SendMoneyFetchAttachment>> PostAttachmentAsync(List<SendMoneyAttachmentFiles> filedata, int transId, CancellationToken cancellationToken)
    {
        List<SendMoneyAttachment> SendMoneyAttachmentList = new List<SendMoneyAttachment>();
        List<SendMoneyFetchAttachment> SendMoneyFetchAttachmentList = new List<SendMoneyFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (SendMoneyAttachmentFiles item in filedata)
            {
                SendMoneyAttachmentList.Add(new SendMoneyAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = transId,
                });
                SendMoneyFetchAttachmentList.Add(new SendMoneyFetchAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = transId,
                });
            }
            await _context.SendMoneyAttachment.AddRangeAsync(SendMoneyAttachmentList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return SendMoneyFetchAttachmentList;
    }

    public async Task<List<SendMoneyFetchTag>> PostTagAsync(List<SendMoneyInsertTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<SendMoneyTag> SendMoneyTag = new List<SendMoneyTag>();
        List<SendMoneyFetchTag> SendMoneyFetchTag = new List<SendMoneyFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (SendMoneyInsertTag item in filedata)
            {
                SendMoneyTag.Add(new SendMoneyTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
                SendMoneyFetchTag.Add(new SendMoneyFetchTag
                {
                    TagId = item.TagId,
                    TransId = TransId
                });
            }
            await _context.SendMoneyTag.AddRangeAsync(SendMoneyTag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return SendMoneyFetchTag;
    }


    public async Task<List<SendMoneyFetchList>> PostSendMoneyListAsync(List<SendMoneyInsertList> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<SendMoneyList> SendMoneyList = new List<SendMoneyList>();
        List<SendMoneyFetchList> SendMoneyFetchList = new List<SendMoneyFetchList>();

        if (filedata.Count > 0)
        {
            foreach (SendMoneyInsertList item in filedata)
            {
                SendMoneyList.Add(new SendMoneyList
                {
                    PriceIncludesTax = item.PriceIncludesTax,
                    AccountId = item.AccountId,
                    Description = item.Description,
                    TaxId = item.TaxId,
                    Amount = item.Amount,
                    TransId = TransId
                });
                SendMoneyFetchList.Add(new SendMoneyFetchList
                {
                    PriceIncludesTax = item.PriceIncludesTax,
                    AccountId = item.AccountId,
                    Description = item.Description,
                    TaxId = item.TaxId,
                    Amount = item.Amount,
                    TransId = TransId
                });
            }
            await _context.SendMoneyList.AddRangeAsync(SendMoneyList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return SendMoneyFetchList;
    }



}


public class SendMoneyInsertDto : IRequest<RowResponse<SendMoneyFetchDto>>
{
    public int PayFromAccountId { get; set; } = 0;
    public int RecipientContactId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
    public Int64 TotalAmount { get; set; } = 0;
    public Int64 WitholdingAmount { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public int DiscountForAccountId { get; set; } = 0;
    public List<SendMoneyAttachmentFiles> AttachmentFile { get; set; }
    public List<SendMoneyInsertTag> TagList { get; set; }
    public List<SendMoneyInsertList> SendMoneyList { get; set; }
}

public class SendMoneyAttachmentFiles
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class SendMoneyInsertTag
{
    public int TagId { get; set; } = 0;
}

public class SendMoneyInsertList
{
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;

}