namespace TabpediaFin.Handler.ReceiveMoneyHandler;

public class ReceiveMoneyFetchHandler : IFetchByIdHandler<ReceiveMoneyFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public ReceiveMoneyFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ReceiveMoneyFetchDto>> Handle(FetchByIdRequestDto<ReceiveMoneyFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<ReceiveMoneyFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<ReceiveMoneyFetchDto>(request.Id, _currentUser);
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

[Table("ReceiveMoney")]
public class ReceiveMoneyFetchDto : BaseDto
{
    public int DepositFromAccountId { get; set; } = 0;
    public int VendorId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public int TransactionNo { get; set; } = 0;
    public bool PriceIncludesTax { get; set; } = false;
    public int ReceiveFromAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public List<ReceiveMoneyFetchTag> TagList { get; set; }
    public List<ReceiveMoneyFetchAttachment> AttachmentList { get; set; }
}

public class ReceiveMoneyFetchAttachment : BaseDto
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public int TransId { get; set; }
}

public class ReceiveMoneyFetchTag : BaseDto
{
    public int TagId { get; set; }
    public int TransId { get; set; }
}