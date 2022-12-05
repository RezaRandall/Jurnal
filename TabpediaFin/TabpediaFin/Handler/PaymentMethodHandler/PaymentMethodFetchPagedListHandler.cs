namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodFetchPagedListHandler : IFetchPagedListHandler<PaymentMethodListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentMethodFetchPagedListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }


    public async Task<PagedListResponse<PaymentMethodListDto>> Handle(FetchPagedListRequestDto<PaymentMethodListDto> request, CancellationToken cancellationToken)
    {
        if (request.PageNum == 0) { request.PageNum = 1; }
        if (request.PageSize == 0) { request.PageSize = 10; }

        var result = new PagedListResponse<PaymentMethodListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<PaymentMethodDto>();
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

                var list = await cn.FetchListPagedAsync<PaymentMethodListDto>(pageNumber: request.PageNum
                    , rowsPerPage: request.PageSize
                    , conditions: sqlWhere
                    , orderby: orderby
                    , currentUser: _currentUser
                    , parameters: parameters);

                int recordCount = await cn.RecordCountAsync<PaymentMethodListDto>(sqlWhere, parameters);

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list?.AsList() ?? new List<PaymentMethodListDto>();
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



[Table("PaymentMethod")]
public class PaymentMethodListDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;

    [Searchable]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
