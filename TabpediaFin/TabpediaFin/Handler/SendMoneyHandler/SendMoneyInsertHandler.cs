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
        DateTime TransDate = TimeZoneInfo.ConvertTimeToUtc(request.TransactionDate);

        var sendMoney = new SendMoney()
        {
            PayFromAccountId = request.PayFromAccountId,
            ReceiverId = request.ReceiverId,
            TransactionDate = TransDate,
            TransactionNo = request.TransactionNo,
            Memo = request.Memo,
            TotalAmount = request.TotalAmount,
            DiscountAmount = request.DiscountAmount,
            DiscountPercent = request.DiscountPercent,
        };

        try
        {
            await _context.SendMoney.AddAsync(sendMoney, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            transIdResult = sendMoney.Id;

            List<SendMoneyFetchAttachment> returnfile = await PostAttachmentAsync(request.AttachmentFile, transIdResult, cancellationToken);
            List<SendMoneyFetchTag> TagListResult = await PostTagAsync(request.TagList, transIdResult, cancellationToken);
            List<SendMoneyFetchList> ReceiveMoneyListResult = await PostSendMoneyListAsync(request.ReceiveMoneyList, transIdResult, cancellationToken);

            var row = new SendMoneyFetchDto()
            {
                PayFromAccountId = sendMoney.PayFromAccountId,
                ReceiverId = sendMoney.ReceiverId,
                TransactionDate = sendMoney.TransactionDate,
                TransactionNo = sendMoney.TransactionNo,
                Memo = sendMoney.Memo,
                TotalAmount = sendMoney.TotalAmount,
                DiscountAmount = sendMoney.DiscountAmount,
                DiscountPercent = sendMoney.DiscountPercent,
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
                    PaymentForAccountCashAndBanktId = item.PaymentForAccountCashAndBanktId,
                    Description = item.Description,
                    TaxId = item.TaxId,
                    Amount = item.Amount,
                    TransId = TransId
                });
                SendMoneyFetchList.Add(new SendMoneyFetchList
                {
                    PriceIncludesTax = item.PriceIncludesTax,
                    PaymentForAccountCashAndBanktId = item.PaymentForAccountCashAndBanktId,
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
    public int ReceiverId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public List<SendMoneyAttachmentFiles> AttachmentFile { get; set; }
    public List<SendMoneyInsertTag> TagList { get; set; }
    public List<SendMoneyInsertList> ReceiveMoneyList { get; set; }
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
    public int PaymentForAccountCashAndBanktId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;

}