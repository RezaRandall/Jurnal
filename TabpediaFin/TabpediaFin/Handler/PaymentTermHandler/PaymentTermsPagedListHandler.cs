using Org.BouncyCastle.Asn1.Ocsp;

namespace TabpediaFin.Handler.PaymentTerm;

public class PaymentTermsPagedListHandler : IFetchPagedListHandler<PaymentTermListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentTermsPagedListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<PaymentTermListDto>> Handle(FetchPagedListRequestDto<PaymentTermListDto> req, CancellationToken cancellationToken)
    {
        if (req.PageNum == 0) { req.PageNum = 1; }
        if (req.PageSize == 0) { req.PageSize = 10; }

        var result = new PagedListResponse<PaymentTermListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<PaymentTermListDto>();
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

                var list = await cn.FetchListPagedAsync<PaymentTermListDto>(pageNumber: req.PageNum
                , rowsPerPage: req.PageSize
                , search: req.Search
                , sortby: req.SortBy
                , sortdesc: req.SortDesc
                , currentUser: _currentUser);
                //int recordCount = await cn.RecordCountAsync<PaymentTermListDto>(sqlWhere, parameters);
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

[Table("PaymentTerm")]
public class PaymentTermListDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
    [Searchable]
    public string Description { get; set; } = string.Empty;
    public int TermDays { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
