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

    public async Task<PagedListResponse<ReceiveMoneyListDto>> Handle(FetchPagedListRequestDto<ReceiveMoneyListDto> req, CancellationToken cancellationToken)
    {
        if (req.PageNum == 0) { req.PageNum = 1; }
        if (req.PageSize == 0) { req.PageSize = 10; }

        var result = new PagedListResponse<ReceiveMoneyListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<ReceiveMoneyListDto>();
                parameters.Add("Search", $"%{req.Search.Trim().ToLowerInvariant()}%");
            }

            //var orderby = string.Empty;
            if (string.IsNullOrWhiteSpace(req.SortBy))
            {
                //orderby = SqlHelper.GenerateOrderBy(req.SortBy, req.SortDesc);
                req.SortBy = SqlHelper.GenerateOrderBy(req.SortBy, req.SortDesc, "CreatedUtc");
            }

            using (var cn = _dbManager.CreateConnection())
            {
                cn.Open();

                var list = await cn.FetchListPagedAsync<ReceiveMoneyListDto>(pageNumber: req.PageNum
                   , rowsPerPage: req.PageSize
                   , search: req.Search
                   , sortby: req.SortBy
                   , sortdesc: req.SortDesc
                   , currentUser: _currentUser
                    );
                int recordCount = await cn.RecordCountAsync<ReceiveMoneyListDto>(sqlWhere, parameters);
                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list.List;
                result.RecordCount = recordCount;
            }
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
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