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
                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("Id", request.Id);

                var sql = @"SELECT * FROM ""SendMoney"" WHERE ""TenantId"" = @TenantId AND ""Id"" = @Id ";
                var result = await cn.QueryFirstOrDefaultAsync<SendMoneyFetchDto>(sql, parameters);

                if (result != null)
                {
                    // TAG
                    var sqlSendMoneyTag = @"SELECT smt.""Id""
                                                ,smt.""TagId""
                                                ,smt.""TransId""
                                                 FROM ""SendMoneyTag"" smt
                                                 INNER JOIN ""SendMoney"" sm ON smt.""TransId"" = sm.""Id"" 
                                                 WHERE sm.""TenantId"" = @TenantId AND sm.""Id"" = @Id ";

                    //var parametersub = new DynamicParameters();
                    //parametersub.Add("TenantId", _currentUser.TenantId);
                    //parametersub.Add("IdItem", request.Id);

                    List<SendMoneyFetchTag> resultSendMoneyTag;
                    resultSendMoneyTag = (await cn.QueryAsync<SendMoneyFetchTag>(sqlSendMoneyTag, parameters).ConfigureAwait(false)).ToList();
                    result.SendMoneyTagList = resultSendMoneyTag;


                    //  ATTACHMENT
                    var sqlSendMoneyAttachment = @"SELECT sma.""Id""
                                        , sma.""FileName""
                                        , sma.""FileUrl""
                                        , sma.""Extension""
                                        , sma.""FileSize""
                                        , sma.""TransId"" 
                                        FROM ""SendMoneyAttachment"" sma
                                        INNER JOIN ""SendMoney"" sm ON sma.""TransId"" = sm.""Id"" 
                                        WHERE sm.""TenantId"" = @TenantId AND sm.""Id"" = @Id ";
                    List<SendMoneyFetchAttachment> resultSendMoneyAttachment;
                    resultSendMoneyAttachment = (await cn.QueryAsync<SendMoneyFetchAttachment>(sqlSendMoneyAttachment, parameters).ConfigureAwait(false)).ToList();
                    result.SendMoneyAttachmentList = resultSendMoneyAttachment;


                    //  LIST AMOUNT
                    var sqlSendMoneyList = @"SELECT sml.""Id""
                                        , sml.""PriceIncludesTax""
                                        , sml.""AccountId""
                                        , sml.""Description""
                                        , sml.""TaxId""
                                        , sml.""Amount"" 
                                        , sml.""TransId"" 
                                        FROM ""SendMoneyList"" sml
                                        INNER JOIN ""SendMoney"" sm ON sml.""TransId"" = sm.""Id"" 
                                        WHERE sm.""TenantId"" = @TenantId AND sm.""Id"" = @Id ";
                    List<SendMoneyFetchList> resultSendMoneyList;
                    resultSendMoneyList = (await cn.QueryAsync<SendMoneyFetchList>(sqlSendMoneyList, parameters).ConfigureAwait(false)).ToList();
                    result.SendMoneyList = resultSendMoneyList;
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

[Table("SendMoney")]
public class SendMoneyFetchDto : BaseDto
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
    public List<SendMoneyFetchTag> SendMoneyTagList { get; set; }
    public List<SendMoneyFetchAttachment> SendMoneyAttachmentList { get; set; }
    public List<SendMoneyFetchList> SendMoneyList { get; set; }
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

public class SendMoneyFetchList : BaseDto
{
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int TransId { get; set; }
}