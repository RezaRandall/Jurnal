namespace TabpediaFin.Handler.CoAHandler
{
    public class AccountListHandler : IQueryPagedListAccountDto<AccountListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public AccountListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<AccountListDto>> Handle(QueryPagedListAccountDto<AccountListDto> request, CancellationToken cancellationToken)
        {
            var response = new PagedListResponse<AccountListDto>();
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
                        sqlsearch = @"AND (LOWER(i.""Name"") LIKE @Search  OR LOWER(i.""AccountNumber"") LIKE @Search  OR LOWER(i.""CategoryId"") LIKE @Search  OR LOWER(i.""AccountParentId"") LIKE @Search  OR LOWER(i.""TaxId"") LIKE @Search  OR LOWER(i.""Description"") LIKE @Search)";
                    }

                    var sql = @"SELECT i.""Id"", ac.""Name"" as CategoryAccount, t.""Name"" as TaxName, i.""Name"", i.""AccountNumber"", i.""CategoryId"", i.""AccountParentId"",i.""TaxId"", i.""Description"", i.""Balance"", i.""IsLocked"", i.""BankId""  FROM ""Account"" i LEFT JOIN ""Tax"" t ON i.""TaxId"" = t.""Id"" LEFT JOIN ""AccountCategory"" ac ON i.""CategoryId"" = ac.""Id""  WHERE i.""TenantId"" = @TenantId " + sqlfiltercategory + sqlsearch + " " + sqlsort + "";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                    List<AccountListDto> result;
                    result = (await cn.QueryAsync<AccountListDto>(sql, parameters).ConfigureAwait(false)).ToList();

                    List<AccountListDto> resultfix = new List<AccountListDto>();
                    List<AccountList> resultchild = new List<AccountList>();
                    foreach (AccountListDto item in result)
                    {
                        if(item.AccountParentId != 0)
                        {
                            int myIndex = result.FindIndex(p => p.Id == item.AccountParentId);
                            resultfix[myIndex].ChildAccountList.Add(new AccountList
                            {
                                Name = item.Name,
                                AccountNumber = item.AccountNumber,
                                CategoryId = item.CategoryId,
                                CategoryAccount = item.CategoryAccount,
                                AccountParentId = item.AccountParentId,
                                TaxId = item.TaxId,
                                TaxName = item.TaxName,
                                Description = item.Description,
                                Balance = item.Balance,
                                IsLocked = item.IsLocked,
                                BankId = item.BankId,
                                Id = item.Id,
                            });
                        }
                        else
                        {
                            resultfix.Add(new AccountListDto {
                                Id = item.Id,
                                Name = item.Name,
                                AccountNumber = item.AccountNumber,
                                CategoryId = item.CategoryId,
                                CategoryAccount = item.CategoryAccount,
                                AccountParentId = item.AccountParentId,
                                TaxId = item.TaxId,
                                TaxName = item.TaxName,
                                Description = item.Description,
                                Balance = item.Balance,
                                IsLocked = item.IsLocked,
                                BankId = item.BankId,
                                ChildAccountList = new List<AccountList>()
                            });
                        }
                    }
                    response.RecordCount = result.Count;
                    response.IsOk = true;
                    response.List = resultfix;
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
    public class AccountListDto : BaseDto
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
        public List<AccountList> ChildAccountList { get; set; }
    }

    public class AccountList : BaseDto
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
}
