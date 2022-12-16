namespace TabpediaFin.Handler.TransferMoneyHandler;

public class TransferMoneyFetchHandler : IFetchByIdHandler<TransferMoneyFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public TransferMoneyFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<TransferMoneyFetchDto>> Handle(FetchByIdRequestDto<TransferMoneyFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<TransferMoneyFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<TransferMoneyFetchDto>(request.Id, _currentUser);
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


[Table("TransferMoney")]
public class TransferMoneyFetchDto : BaseDto
{
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; }
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public List<TransferMoneyFetchAttachment> AttachmentList { get; set; }
    public List<TransferMoneyFetchTag> TagList { get; set; }
}

public class TransferMoneyFetchAttachment : BaseDto
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public int TransId { get; set; }
}

public class TransferMoneyFetchTag : BaseDto
{
    public int TagId { get; set; }
    public int TransId { get; set; }
}