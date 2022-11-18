using TabpediaFin.Infrastructure.Utility;

namespace TabpediaFin.Handler.PaymentMethod;

public class PaymentMethodFetchPagedListHandler : IQueryPagedListHandler<PaymentMethodDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentMethodFetchPagedListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }


    public async Task<PagedListResponse<PaymentMethodDto>> Handle(QueryPagedListDto<PaymentMethodDto> request, CancellationToken cancellationToken)
    {
        if (request.PageNum == 0) { request.PageNum = 1; }
        if (request.PageSize == 0) { request.PageSize = 10; }

        var result = new PagedListResponse<PaymentMethodDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<PaymentMethodDto>();
                parameters.Add("Search", request.Search.Trim().ToLowerInvariant());
            }

            var orderby = string.Empty;
            if (string.IsNullOrWhiteSpace(request.SortBy))
            {
                orderby = SqlHelper.GenerateOrderBy(request.SortBy, request.SortDesc);
            }

            using (var cn = _dbManager.CreateConnection())
            {
                cn.Open();

                var list = await cn.GetListPagedAsync<PaymentMethodDto>(pageNumber: request.PageNum
                    , rowsPerPage: request.PageSize
                    , conditions: sqlWhere
                    , orderby: orderby
                    , parameters: parameters);

                int recordCount = await cn.RecordCountAsync<PaymentMethodDto>(sqlWhere, parameters);

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list?.AsList() ?? new List<PaymentMethodDto>();
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
