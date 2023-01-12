using TabpediaFin.Handler.ExpenseHandler;

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
                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("Id", request.Id);

                var sql = @"SELECT * FROM ""ReceiveMoney"" WHERE ""TenantId"" = @TenantId AND ""Id"" = @Id ";
                var result = await cn.QueryFirstOrDefaultAsync<ReceiveMoneyFetchDto>(sql, parameters);

                if (result != null)
                {
                    // TAG
                    var sqlReceiveMoneyTag = @"SELECT rmt.""Id""
                                                ,rmt.""TagId""
                                                ,rmt.""TransId""
                                                 FROM ""ReceiveMoneyTag"" rmt
                                                 INNER JOIN ""ReceiveMoney"" rm ON rmt.""TransId"" = rm.""Id"" 
                                                 WHERE rm.""TenantId"" = @TenantId AND rm.""Id"" = @Id ";

                    //var parametersub = new DynamicParameters();
                    //parametersub.Add("TenantId", _currentUser.TenantId);
                    //parametersub.Add("IdItem", request.Id);

                    List<ReceiveMoneyFetchTag> resultReceiveMoneyTag;
                    resultReceiveMoneyTag = (await cn.QueryAsync<ReceiveMoneyFetchTag>(sqlReceiveMoneyTag, parameters).ConfigureAwait(false)).ToList();
                    result.ReceiveMoneyTagList = resultReceiveMoneyTag;


                    // ATTACHMENT
                    var sqlReceiveMoneyAttachment = @"SELECT rma.""Id""
                                        , rma.""FileName""
                                        , rma.""FileUrl""
                                        , rma.""Extension""
                                        , rma.""FileSize""
                                        , rma.""TransId"" 
                                        FROM ""ReceiveMoneyAttachment"" rma
                                        INNER JOIN ""ReceiveMoney"" rm ON rma.""TransId"" = rm.""Id"" 
                                        WHERE rm.""TenantId"" = @TenantId AND rm.""Id"" = @Id ";
                    List<ReceiveMoneyFetchAttachment> resultReceiveMoneyAttachment;
                    resultReceiveMoneyAttachment = (await cn.QueryAsync<ReceiveMoneyFetchAttachment>(sqlReceiveMoneyAttachment, parameters).ConfigureAwait(false)).ToList();
                    result.ReceiveMoneyAttachmentList = resultReceiveMoneyAttachment;


                    // LIST
                    var sqlReceiveMoneyList = @"SELECT rml.""Id""
                                        , rml.""PriceIncludesTax""
                                        , rml.""AccountId""
                                        , rml.""Description""
                                        , rml.""TaxId""
                                        , rml.""Amount""
                                        , rml.""TransId"" 
                                        FROM ""ReceiveMoneyList"" rml
                                        INNER JOIN ""ReceiveMoney"" rm ON rml.""TransId"" = rm.""Id"" 
                                        WHERE rm.""TenantId"" = @TenantId AND rm.""Id"" = @Id ";
                    List<ReceiveMoneyFetchList> resultReceiveMoneyList;
                    resultReceiveMoneyList = (await cn.QueryAsync<ReceiveMoneyFetchList>(sqlReceiveMoneyList, parameters).ConfigureAwait(false)).ToList();
                    result.ReceiveMoneyList = resultReceiveMoneyList;

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

[Table("ReceiveMoney")]
public class ReceiveMoneyFetchDto : BaseDto
{
    public int DepositToAccountId { get; set; } = 0;
    public int PayerId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
    public Int64 TotalAmount { get; set; } = 0;
    public List<ReceiveMoneyFetchTag> ReceiveMoneyTagList { get; set; }
    public List<ReceiveMoneyFetchAttachment> ReceiveMoneyAttachmentList { get; set; }
    public List<ReceiveMoneyFetchList> ReceiveMoneyList { get; set; }
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

public class ReceiveMoneyFetchList : BaseDto
{
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int TransId { get; set; }
}