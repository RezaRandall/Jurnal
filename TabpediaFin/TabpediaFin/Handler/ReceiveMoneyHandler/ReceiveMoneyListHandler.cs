namespace TabpediaFin.Handler.ReceiveMoneyHandler;  

public class ReceiveMoneyListHandler : IFetchPagedListHandler<ReceiveMoneyListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public ReceiveMoneyListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<ReceiveMoneyListDto>> Handle(FetchPagedListRequestDto<ReceiveMoneyListDto> request, CancellationToken cancellationToken)
    {
        if (request.PageNum == 0) { request.PageNum = 0; }
        if (request.PageSize == 0) { request.PageSize = 10; }

        var response = new PagedListResponse<ReceiveMoneyListDto>();

        try
        {
            using (var conn = _dbManager.CreateConnection())
            {
                string sqlsort = "";
                string sqlsearch = "";
                string expensefilter = "";

                if (request.SortBy != null && request.SortBy != "")
                {
                    sqlsort = @" order by """ + request.SortBy + "\" ASC";

                    if (request.SortDesc == true)
                    {
                        sqlsort = @" order by """ + request.SortBy + "\" ASC";
                    }
                }

                if (request.Search != null && request.Search != "")
                {
                    sqlsearch = @"AND LOWER(""TransactionNo"") LIKE @Search OR LOWER(""Memo"") LIKE @Search ";
                }

                var sql = @"SELECT  *
                          FROM ""ReceiveMoney"" WHERE ""TenantId"" = @TenantId " + expensefilter + " " + sqlsearch + " " + sqlsort + " LIMIT @PageSize OFFSET @PageNum";

                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("PageSize", request.PageSize);
                parameters.Add("PageNum", request.PageNum);
                parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                List<ReceiveMoneyListDto> result;
                result = (await conn.QueryAsync<ReceiveMoneyListDto>(sql, parameters).ConfigureAwait(false)).ToList();

                response.RecordCount = result.Count;

                response.IsOk = true;
                response.List = result;
                response.ErrorMessage = string.Empty;
            }
        }
        catch (Exception ex)
        {
            response.IsOk = false;
            response.ErrorMessage = ex.Message;
        }
        return response;
    }

}


[Table("ReceiveMoney")]
public class ReceiveMoneyListDto : BaseDto
{
    public int DepositToAccountId { get; set; } = 0;
    public int PayerId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    [Searchable]
    public string TransactionNo { get; set; } = string.Empty;
    [Searchable]
    public string Memo { get; set; } = string.Empty;
    public Int64 TotalAmount { get; set; } = 0;
}