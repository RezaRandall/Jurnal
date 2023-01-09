namespace TabpediaFin.Handler.TransferMoneyHandler;

public class TransferMoneyListHandler : IFetchPagedListHandler<TransferMoneyListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public TransferMoneyListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<TransferMoneyListDto>> Handle(FetchPagedListRequestDto<TransferMoneyListDto> request, CancellationToken cancellationToken)
    {
        if (request.PageNum == 0) { request.PageNum = 0; }
        if (request.PageSize == 0) { request.PageSize = 10; }

        var response = new PagedListResponse<TransferMoneyListDto>();

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
                    sqlsearch = @"AND LOWER(""TransactionNumber"") LIKE @Search OR LOWER(""Memo"") LIKE @Search ";
                }

                var sql = @"SELECT  *
                          FROM ""TransferMoney"" WHERE ""TenantId"" = @TenantId " + expensefilter + " " + sqlsearch + " " + sqlsort + " LIMIT @PageSize OFFSET @PageNum";

                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("PageSize", request.PageSize);
                parameters.Add("PageNum", request.PageNum);
                parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                List<TransferMoneyListDto> result;
                result = (await conn.QueryAsync<TransferMoneyListDto>(sql, parameters).ConfigureAwait(false)).ToList();

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

[Table("TransferMoney")]
public class TransferMoneyListDto : BaseDto
{
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    [Searchable]
    public string Memo { get; set; } = string.Empty;
    [Searchable]
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
}