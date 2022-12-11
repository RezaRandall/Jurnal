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
            using (var cn = _dbManager.CreateConnection())
            {
                cn.Open();

                var q = await cn.FetchListPagedAsync<PaymentMethodListDto>(pageNumber: request.PageNum
                    , rowsPerPage: request.PageSize
                    , search: request.Search
                    , sortby: request.SortBy
                    , sortdesc: request.SortDesc
                    , currentUser: _currentUser);

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = q.List;
                result.RecordCount = q.TotalRecord;
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
