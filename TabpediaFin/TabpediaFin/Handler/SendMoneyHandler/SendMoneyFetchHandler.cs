using TabpediaFin.Handler.ReceiveMoneyHandler;

namespace TabpediaFin.Handler.SendMoneyHandler;

public class SendMoneyFetchHandler : IFetchByIdHandler<SendMoneyFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public SendMoneyFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<SendMoneyFetchDto>> Handle(FetchByIdRequestDto<SendMoneyFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<SendMoneyFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<SendMoneyFetchDto>(request.Id, _currentUser);
                if (row == null)
                {
                    response.IsOk = false;
                    response.Row = row;
                    response.ErrorMessage = "Data not found";
                }
                else
                {
                    response.IsOk = true;
                    response.Row = row;
                    response.ErrorMessage = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            response.IsOk = false;
            response.Row = null;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}

[Table("SendMoney")]
public class SendMoneyFetchDto : BaseDto
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
    public List<SendMoneyFetchTag> TagList { get; set; }
    public List<SendMoneyFetchAttachment> AttachmentList { get; set; }
}

public class SendMoneyFetchAttachment : BaseDto
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public int TransId { get; set; }
}

public class SendMoneyFetchTag : BaseDto
{
    public int TagId { get; set; }
    public int TransId { get; set; }
}