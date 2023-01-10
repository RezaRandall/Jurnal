namespace TabpediaFin.Handler.CoAHandler
{
    public class AccountChildListHandler : IQueryPagedChildListAccountDto<AccountChildListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public AccountChildListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<AccountChildListDto>> Handle(QueryPagedChildListAccountDto<AccountChildListDto> request, CancellationToken cancellationToken)
        {
            var response = new PagedListResponse<AccountChildListDto>();
            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    string sqlsort = "";
                    string sqlsearch = "";
                    string sqlfilterchild = @"AND i.""AccountParentId"" = " + request.parentAccountId + "";
                    
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
                        sqlsearch = @"AND (LOWER(i.""Name"") LIKE @Search  OR LOWER(i.""AccountNumber"") LIKE @Search  OR LOWER(i.""CategoryId"") LIKE @Search  OR LOWER(i.""AccountParentId"") LIKE @Search  OR LOWER(i.""TaxId"") LIKE @Search  OR LOWER(i.""Description"") LIKE @Search)";
                    }

                    var sql = @"SELECT ac.""Name"" as CategoryAccount, t.""Name"" as TaxName, i.""Name"", i.""AccountNumber"", i.""CategoryId"", i.""AccountParentId"", i.""TaxId"", i.""Description"", i.""Balance"", i.""IsLocked"", i.""BankId""  FROM ""Account"" i LEFT JOIN ""Tax"" t ON i.""TaxId"" = t.""Id"" LEFT JOIN ""AccountCategory"" ac ON i.""CategoryId"" = ac.""Id""  WHERE i.""TenantId"" = @TenantId " + sqlfilterchild + sqlsearch + " " + sqlsort + "";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                    List<AccountChildListDto> result;
                    result = (await cn.QueryAsync<AccountChildListDto>(sql, parameters).ConfigureAwait(false)).ToList();

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
    public class AccountChildListDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public int CategoryId { get; set; } = 0;
        public string CategoryAccount { get; set; } = string.Empty;
        public int AccountParentId { get; set; } = 0;
        public int TaxId { get; set; } = 0;
        public string TaxName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Balance { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public int BankId { get; set; } = 0;
    }
    public class QueryPagedChildListAccountDto<T> : IRequest<PagedListResponse<T>>
    {
        public string Search { get; set; } = string.Empty;

        public string SortBy { get; set; } = string.Empty;

        public bool SortDesc { get; set; }
        public int parentAccountId { get; set; } = 0;
    }

    public interface IQueryPagedChildListAccountDto<T> : IRequestHandler<QueryPagedChildListAccountDto<T>, PagedListResponse<T>>
    {
    }
}
