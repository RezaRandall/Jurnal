using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Domain.SendMoney;
using TabpediaFin.Domain.TransferMoney;
using TabpediaFin.Handler.ReceiveMoneyHandler;

namespace TabpediaFin.Handler.SendMoneyHandler;

public class SendMoneyUpdateHandler : IRequestHandler<SendMoneyUpdateDto, RowResponse<SendMoneyFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public SendMoneyUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<SendMoneyFetchDto>> Handle(SendMoneyUpdateDto request, CancellationToken cancellationToken)
    {
        int sendMoneyId;
        var result = new RowResponse<SendMoneyFetchDto>();
        List<SendMoneyTag> SendMoneyAttachment = new List<SendMoneyTag>();
        List<SendMoneyFetchAttachment> SendMoneyFetchAttachment = new List<SendMoneyFetchAttachment>();

        try
        {
            var sendMoney = await _context.SendMoney.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            sendMoney.PayFromAccountId = request.PayFromAccountId;
            sendMoney.ReceiverVendorId = request.ReceiverVendorId;
            sendMoney.TransactionDate = request.TransactionDate;
            sendMoney.TransactionNo = request.TransactionNo;
            sendMoney.PriceIncludesTax = request.PriceIncludesTax;
            sendMoney.AccountCashAndBankId = request.AccountCashAndBankId;
            sendMoney.Description = request.Description;
            sendMoney.TaxId = request.TaxId;
            sendMoney.Amount = request.Amount;
            sendMoney.Memo = request.Memo;
            sendMoney.TotalAmount = request.TotalAmount;
            sendMoney.DiscountAmount = request.DiscountAmount;
            sendMoney.DiscountPercent = request.DiscountPercent;

            await _context.SaveChangesAsync(cancellationToken);
            sendMoneyId = request.Id;

            List<SendMoneyFetchAttachment> returnfile = await UpdateAttachmentAsync(request.AttachmentFile, sendMoneyId, cancellationToken);
            List<SendMoneyFetchTag> TagListResult = await UpdateTagAsync(request.TagList, sendMoneyId, cancellationToken);

            var row = new SendMoneyFetchDto()
            {
                Id = request.Id,
                PayFromAccountId = request.PayFromAccountId,
                ReceiverVendorId = request.ReceiverVendorId,
                TransactionDate = request.TransactionDate,
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

    public async Task<List<SendMoneyFetchAttachment>> UpdateAttachmentAsync(List<SendMoneyAttachmentUpdate> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<SendMoneyAttachment> SendMoneyAttachmentList = new List<SendMoneyAttachment>();
        List<SendMoneyFetchAttachment> SendMoneyFetchAttachmentList = new List<SendMoneyFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (SendMoneyAttachmentUpdate item in filedata)
            {
                SendMoneyAttachmentList.Add(new SendMoneyAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
                SendMoneyFetchAttachmentList.Add(new SendMoneyFetchAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    TransId = TransId,
                });
            }

            _context.SendMoneyAttachment.UpdateRange(SendMoneyAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return SendMoneyFetchAttachmentList;
    }

    public async Task<List<SendMoneyFetchTag>> UpdateTagAsync(List<SendMoneyUpdateTag> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<SendMoneyTag> SendMoneyTag = new List<SendMoneyTag>();
        List<SendMoneyFetchTag> SendMoneyFetchTag = new List<SendMoneyFetchTag>();

        if (filedata.Count > 0)
        {
            foreach (SendMoneyUpdateTag item in filedata)
            {
                SendMoneyTag.Add(new SendMoneyTag
                {
                    Id = item.Id,
                    TagId = item.TagId,
                    TransId = TransId
                });
                SendMoneyFetchTag.Add(new SendMoneyFetchTag
                {
                    Id = item.Id,
                    TagId = item.TagId,
                    TransId = TransId
                });
            }
            _context.SendMoneyTag.UpdateRange(SendMoneyTag);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return SendMoneyFetchTag;
    }



}

public class SendMoneyUpdateDto : IRequest<RowResponse<SendMoneyFetchDto>>
{
    public int Id { get; set; }
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
    public List<SendMoneyAttachmentUpdate> AttachmentFile { get; set; }
    public List<SendMoneyUpdateTag> TagList { get; set; }
}

public class SendMoneyAttachmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}

public class SendMoneyUpdateTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}