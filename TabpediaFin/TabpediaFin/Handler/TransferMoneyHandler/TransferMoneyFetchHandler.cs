using TabpediaFin.Handler.TransferMoneyHandler;

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
                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("Id", request.Id);

                var sql = @"SELECT * FROM ""TransferMoney"" WHERE ""TenantId"" = @TenantId AND ""Id"" = @Id ";
                var result = await cn.QueryFirstOrDefaultAsync<TransferMoneyFetchDto>(sql, parameters);

                if (result != null)
                {
                    var sqlTransferMoneyTag = @"SELECT tmt.""Id""
                                                ,tmt.""TagId""
                                                ,tmt.""TransId""
                                                 FROM ""TransferMoneyTag"" tmt
                                                 INNER JOIN ""TransferMoney"" sm ON tmt.""TransId"" = sm.""Id"" 
                                                 WHERE sm.""TenantId"" = @TenantId AND sm.""Id"" = @Id ";


                    List<TransferMoneyFetchTag> resultTransferMoneyTag;
                    resultTransferMoneyTag = (await cn.QueryAsync<TransferMoneyFetchTag>(sqlTransferMoneyTag, parameters).ConfigureAwait(false)).ToList();
                    result.TransferMoneyTagList = resultTransferMoneyTag;

                    var sqlTransferMoneyAttachment = @"SELECT sma.""Id""
                                        , sma.""FileName""
                                        , sma.""FileUrl""
                                        , sma.""Extension""
                                        , sma.""FileSize""
                                        , sma.""TransId"" 
                                        FROM ""TransferMoneyAttachment"" sma
                                        INNER JOIN ""TransferMoney"" sm ON sma.""TransId"" = sm.""Id"" 
                                        WHERE sm.""TenantId"" = @TenantId AND sm.""Id"" = @Id ";
                    List<TransferMoneyFetchAttachment> resultTransferMoneyAttachment;
                    resultTransferMoneyAttachment = (await cn.QueryAsync<TransferMoneyFetchAttachment>(sqlTransferMoneyAttachment, parameters).ConfigureAwait(false)).ToList();
                    result.TransferMoneyAttachmentList = resultTransferMoneyAttachment;

                }

                response.IsOk = true;
                response.Row = result;
                response.ErrorMessage = string.Empty;
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
    public Int64 Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public List<TransferMoneyFetchAttachment> TransferMoneyAttachmentList { get; set; }
    public List<TransferMoneyFetchTag> TransferMoneyTagList { get; set; }
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