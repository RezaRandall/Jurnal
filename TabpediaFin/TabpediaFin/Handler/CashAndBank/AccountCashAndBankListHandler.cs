using TabpediaFin.Handler.ExpenseCategoryHandler;

namespace TabpediaFin.Handler.CashAndBank;

public class AccountCashAndBankListHandler : IQueryPagedListHandler<AccountCashAndBankListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public AccountCashAndBankListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<AccountCashAndBankListDto>> Handle(QueryPagedListDto<AccountCashAndBankListDto> request, CancellationToken cancellationToken)
    {
        if (request.PageNum == 0) { request.PageNum = 1; }
        if (request.PageSize == 0) { request.PageSize = 10; }

        var result = new PagedListResponse<AccountCashAndBankListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<AccountCashAndBankListDto>();
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

                var list = await cn.FetchListPagedAsync<AccountCashAndBankListDto>(pageNumber: request.PageNum
                    , rowsPerPage: request.PageSize
                    , conditions: sqlWhere
                    , orderby: orderby
                    , currentUser: _currentUser
                    , parameters: parameters);

                int recordCount = await cn.RecordCountAsync<AccountCashAndBankListDto>(sqlWhere, parameters);

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list?.AsList() ?? new List<AccountCashAndBankListDto>();
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


[Table("AccountCashAndBank")]
public class AccountCashAndBankListDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public int CashAndBankCategoryId { get; set; } = 0;
    public int DetailAccountId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    public int BankId { get; set; } = 0;
    [Searchable]
    public string Description { get; set; } = string.Empty;
}
