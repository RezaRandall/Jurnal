using TabpediaFin.Handler.CashAndBank;

namespace TabpediaFin.Handler.BankNameHandler;

public class BankNameListHandler : IFetchPagedListHandler<BankNameListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public BankNameListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<BankNameListDto>> Handle(FetchPagedListRequestDto<BankNameListDto> request, CancellationToken cancellationToken)
    {
        if (request.PageNum == 0) { request.PageNum = 1; }
        if (request.PageSize == 0) { request.PageSize = 10; }

        var result = new PagedListResponse<BankNameListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<BankNameListDto>();
                parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");
            }

            var orderby = string.Empty;
            if (string.IsNullOrWhiteSpace(request.SortBy))
            {
                orderby = SqlHelper.GenerateOrderBy(request.SortBy, request.SortDesc);
            }

            using (var cn = _dbManager.CreateConnection())
            {
                cn.Open();

                var list = await cn.FetchListPagedAsync<BankNameListDto>(pageNumber: request.PageNum
                    , rowsPerPage: request.PageSize
                    , search: request.Search
                    , sortby: request.SortBy
                    , sortdesc: request.SortDesc
                    , currentUser: _currentUser);

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list.List;
                result.RecordCount = list.TotalRecord;
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

[Table("BankName")]
public class BankNameListDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
}
