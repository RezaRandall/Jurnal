using TabpediaFin.Domain.SendMoney;
using TabpediaFin.Handler.ExpenseHandler;
using TabpediaFin.Handler.UploadAttachmentHandler;

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
            ReceiverVendorId = request.ReceiverVendorId,
            TransactionDate = TransDate,
            TransactionNo = request.TransactionNo,
            PriceIncludesTax = request.PriceIncludesTax,
            AccountCashAndBankId = request.AccountCashAndBankId,
            Description = request.Description,
            TaxId = request.TaxId,
            Amount = request.Amount,
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

            var row = new SendMoneyFetchDto()
            {
                PayFromAccountId = sendMoney.PayFromAccountId,
                ReceiverVendorId = sendMoney.ReceiverVendorId,
                TransactionDate = sendMoney.TransactionDate,
                TransactionNo = sendMoney.TransactionNo,
                PriceIncludesTax = sendMoney.PriceIncludesTax,
                AccountCashAndBankId = sendMoney.AccountCashAndBankId,
                Description = sendMoney.Description,
                TaxId = sendMoney.TaxId,
                Amount = sendMoney.Amount,
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



}


public class SendMoneyInsertDto : IRequest<RowResponse<SendMoneyFetchDto>>
{
    public int PayFromAccountId { get; set; } = 0;
    public int ReceiverVendorId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public int TransactionNo { get; set; } = 0;
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountCashAndBankId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public List<SendMoneyAttachmentFiles> AttachmentFile { get; set; }
    public List<SendMoneyInsertTag> TagList { get; set; }
}

public class SendMoneyAttachmentFiles
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int TransId { get; set; }
}

public class SendMoneyInsertTag
{
    public int TagId { get; set; } = 0;
}