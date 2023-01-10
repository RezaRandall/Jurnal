namespace TabpediaFin.Handler.CashAndBank;

public class AccountListsHandler : IQueryPagedListAccountDto<AccountListsDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public AccountListsHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<AccountListsDto>> Handle(QueryPagedListAccountDto<AccountListsDto> request, CancellationToken cancellationToken)
    {
        var response = new PagedListResponse<AccountListsDto>();
        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                string sqlsort = "";
                string sqlsearch = "";
                string sqlfiltercategory = "";
                if (request.category != null && request.category != 0)
                {
                    sqlfiltercategory = @"AND i.""CategoryId"" = " + request.category + "";
                }
                if (request.SortBy != null && request.SortBy != "")
                {
                    sqlsort = @" order by """ + request.SortBy + "\" ASC";

                    if (request.SortDesc == true)
                    {
                        sqlsort = @" order by """ + request.SortBy + "\" ASC";
                    }
                }

                if (request.Search != null && request.Search != "")
                {
                    sqlsearch = @"AND (LOWER(i.""Name"") LIKE @Search  OR LOWER(i.""AccountNumber"") LIKE @Search  OR LOWER(i.""Description"") LIKE @Search)";
                }

                var sql = @"SELECT i.""Id"" as ID, ac.""Name"" as CategoryAccount, t.""Name"" as TaxName, i.""Name"", i.""AccountNumber"", i.""CategoryId"", i.""AccountParentId"",i.""TaxId"", i.""Description"", i.""Balance"", i.""IsLocked"", i.""BankId""  FROM ""Account"" i LEFT JOIN ""Tax"" t ON i.""TaxId"" = t.""Id"" LEFT JOIN ""AccountCategory"" ac ON i.""CategoryId"" = ac.""Id""  WHERE i.""TenantId"" = @TenantId " + sqlfiltercategory + sqlsearch + " " + sqlsort + "";

                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                List<AccountListsDto> result;
                result = (await cn.QueryAsync<AccountListsDto>(sql, parameters).ConfigureAwait(false)).ToList();

                response.RecordCount = result.Count;

                response.IsOk = true;
                response.List = result;
                response.ErrorMessage = string.Empty;
            }
        }
        catch (Exception ex)
        {
            response.IsOk = false;
            response.List = null;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}


[Table("Account")]
public class AccountListsDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
    [Searchable]
    public string AccountNumber { get; set; } = string.Empty;
    public int CategoryId { get; set; } = 0;
    public int AccountParentId { get; set; } = 0;
    public int BankId { get; set; } = 0;
    public int TaxId { get; set; } = 0;
    [Searchable]
    public string Description { get; set; } = string.Empty;
    public Int64 Balance { get; set; } = 0;
    public bool IsLocked { get; set; } = false;
}

public class QueryPagedListAccountDto<T> : IRequest<PagedListResponse<T>>
{
    public string Search { get; set; } = string.Empty;

    public string SortBy { get; set; } = string.Empty;

    public bool SortDesc { get; set; }
    public int category { get; set; } = 0;
}
public interface IQueryPagedListAccountDto<T> : IRequestHandler<QueryPagedListAccountDto<T>, PagedListResponse<T>>
{
}