using TabpediaFin.Handler.Item;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseListHandler : IFetchPagedListHandler<ExpenseListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public ExpenseListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<ExpenseListDto>> Handle(FetchPagedListRequestDto<ExpenseListDto> req, CancellationToken cancellationToken)
    {
        if (req.PageNum == 0) { req.PageNum = 1; }
        if (req.PageSize == 0) { req.PageSize = 10; }

        var result = new PagedListResponse<ExpenseListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<ExpenseListDto>();
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

                var list = await cn.FetchListPagedAsync<ExpenseListDto>(pageNumber: req.PageNum
                    , rowsPerPage: req.PageSize
                    , conditions: sqlWhere
                    , orderby: orderby
                    , currentUser: _currentUser
                    , parameters: parameters
                    );
                int recordCount = await cn.RecordCountAsync<ExpenseListDto>(sqlWhere, parameters);
                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list?.AsList() ?? new List<ExpenseListDto>();
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

[Table("Expense")]
public class ExpenseListDto : BaseDto
{
    [Searchable]
    public string TransNum { get; set; } = string.Empty;
    public DateTime? TransDate { get; set; }
    public int ContactId { get; set; } = 0;
    public int PaymentMethodId { get; set; } = 0;
    public int PaymentTermId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int DiscountTypeId { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    [Searchable]
    public string Notes { get; set; } = string.Empty;
    [Searchable]
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
}