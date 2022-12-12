namespace TabpediaFin.Handler.UnitMeasures;

public class UnitMeasurePagedListHandler : IFetchPagedListHandler<UnitMeasureListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public UnitMeasurePagedListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<UnitMeasureListDto>> Handle(FetchPagedListRequestDto<UnitMeasureListDto> req, CancellationToken cancellationToken)
    {
        if (req.PageNum == 0) { req.PageNum = 1; }
        if (req.PageSize == 0) { req.PageSize = 10; }

        var result = new PagedListResponse<UnitMeasureListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<UnitMeasureListDto>();
                parameters.Add("Search", $"%{req.Search.Trim().ToLowerInvariant()}%");
            }

            var orderby = string.Empty;
            if (string.IsNullOrWhiteSpace(req.SortBy))
            {
                orderby = SqlHelper.GenerateOrderBy(req.SortBy, req.SortDesc);
            }

            using (var cn = _dbManager.CreateConnection())
            {
                cn.Open();

                var list = await cn.FetchListPagedAsync<UnitMeasureListDto>(pageNumber: req.PageNum
                , rowsPerPage: req.PageSize
                , search: req.Search
                , sortby: req.SortBy
                , sortdesc: req.SortDesc
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



[Table("UnitMeasure")]
public class UnitMeasureListDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
    [Searchable]
    public string Description { get; set; } = string.Empty;
}
