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

    public async Task<PagedListResponse<TransferMoneyListDto>> Handle(FetchPagedListRequestDto<TransferMoneyListDto> req, CancellationToken cancellationToken)
    {
        if (req.PageNum == 0) { req.PageNum = 1; }
        if (req.PageSize == 0) { req.PageSize = 10; }

        var result = new PagedListResponse<TransferMoneyListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<TransferMoneyListDto>();
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

                var list = await cn.FetchListPagedAsync<TransferMoneyListDto>(pageNumber: req.PageNum
                , rowsPerPage: req.PageSize
                , search: req.Search
                , sortby: req.SortBy
                , sortdesc: req.SortDesc
                , currentUser: _currentUser
                    );
                int recordCount = await cn.RecordCountAsync<TransferMoneyListDto>(sqlWhere, parameters);
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








[Table("TransferMoney")]
public class TransferMoneyListDto : BaseDto
{
    public int TransferFromAccountId { get; set; } = 0;
    public int DepositToAccountId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    [Searchable]
    public string Memo { get; set; } = string.Empty;
    [Searchable]
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
}