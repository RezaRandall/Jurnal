using TabpediaFin.Domain.ReceiveMoney;
using TabpediaFin.Domain.SendMoney;
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

        List<SendMoneyTag> sendMoneyTag = new List<SendMoneyTag>();
        List<SendMoneyAttachment> sendMoneyAttachment = new List<SendMoneyAttachment>();
        List<SendMoneyFetchTag> sendMoneyFetchTag = new List<SendMoneyFetchTag>();
        List<SendMoneyFetchAttachment> sendMoneyFetchAttachment = new List<SendMoneyFetchAttachment>();

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

            sendMoneyId = request.Id;
            List<int> idUpdateSendMoneyTag = new List<int>();
            List<int> idUpdateSendMoneyAttachment = new List<int>();

            if (request.SendMoneyTagList.Count > 0)
            {
                foreach (SendMoneyUpdateTag i in request.SendMoneyTagList)
                {
                    idUpdateSendMoneyTag.Add(i.Id);
                    sendMoneyTag.Add(new SendMoneyTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = sendMoneyId,
                        CreatedUid = _currentUser.UserId
                    });
                    sendMoneyFetchTag.Add(new SendMoneyFetchTag
                    {
                        Id = i.Id,
                        TagId = i.TagId,
                        TransId = sendMoneyId
                    });
                }
                _context.SendMoneyTag.UpdateRange(sendMoneyTag);
            }

            if (request.SendMoneyAttachmentFile.Count > 0)
            {
                foreach (SendMoneyAttachmentUpdate i in request.SendMoneyAttachmentFile)
                {
                    idUpdateSendMoneyAttachment.Add(i.Id);
                    sendMoneyAttachment.Add(new SendMoneyAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        CreatedUid = _currentUser.UserId,
                        TransId = sendMoneyId
                    });
                    sendMoneyFetchAttachment.Add(new SendMoneyFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        TransId = sendMoneyId
                    });
                }
                _context.SendMoneyAttachment.UpdateRange(sendMoneyAttachment);
            }

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
                SendMoneyTagList = sendMoneyFetchTag,
                SendMoneyAttachmentList = sendMoneyFetchAttachment,
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

public class SendMoneyUpdateDto : IRequest<RowResponse<SendMoneyFetchDto>>
{
    public int Id { get; set; }
    public int PayFromAccountId { get; set; } = 0;
    public int ReceiverVendorId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountCashAndBankId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public List<SendMoneyAttachmentUpdate> SendMoneyAttachmentFile { get; set; }
    public List<SendMoneyUpdateTag> SendMoneyTagList { get; set; }
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