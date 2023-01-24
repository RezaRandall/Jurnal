namespace TabpediaFin.Handler.CoAHandler
{
    public class AccountPurchaseSalesListHandler : IRequestHandler<AccountPurchaseSalesList, RowResponse<AccountPurchaseSalesListDto>>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public AccountPurchaseSalesListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<AccountPurchaseSalesListDto>> Handle(AccountPurchaseSalesList request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<AccountPurchaseSalesListDto>();
            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sql = @"SELECT i.""Id"", i.""Name"", i.""AccountNumber"", i.""CategoryId"", i.""AccountParentId"", i.""TaxId"", i.""Description"", i.""Balance"", i.""IsLocked"", i.""BankId""  FROM ""Account"" i LEFT JOIN ""Tax"" t ON i.""TaxId"" = t.""Id"" LEFT JOIN ""AccountCategory"" ac ON i.""CategoryId"" = ac.""Id""  WHERE i.""TenantId"" = @TenantId ";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);

                    List<AccountData> query;
                    query = (await cn.QueryAsync<AccountData>(sql, parameters).ConfigureAwait(false)).ToList();

                    AccountPurchaseSalesListDto result = new AccountPurchaseSalesListDto();
                    result.PurchaseAccount = new AccountFetchDataList();
                    result.SalesAccount = new AccountFetchDataList();
                    result.StockAccount = new AccountFetchDataList();

                    List<AccountData>  tempPurchaseAccount = new List<AccountData>();
                    List<AccountData> tempSalesAccount = new List<AccountData>();
                    List<AccountData> tempStockAccount = new List<AccountData>();

                    foreach (AccountData item in query)
                    {
                        if (item.AccountNumber == "5-50000") { 
                            result.PurchaseAccount.defaultAccountId = item.Id; 
                        }

                        if (item.CategoryId == 11)
                        {
                            tempPurchaseAccount.Add(new AccountData
                            {
                                Id = item.Id,
                                Name = item.Name,
                                AccountNumber = item.AccountNumber,
                                TaxId = item.TaxId,
                                TaxName = item.TaxName,
                                Description = item.Description,
                                CategoryId = item.CategoryId,
                            });
                        }

                        if (item.AccountNumber == "4-40000") { 
                            result.SalesAccount.defaultAccountId = item.Id; 
                        }

                        if (item.CategoryId == 12)
                        {
                            tempSalesAccount.Add(new AccountData
                            {
                                Id = item.Id,
                                Name = item.Name,
                                AccountNumber = item.AccountNumber,
                                TaxId = item.TaxId,
                                TaxName = item.TaxName,
                                Description = item.Description,
                                CategoryId = item.CategoryId,
                            });
                        }

                        if (item.AccountNumber == "1-10200") { 
                            result.StockAccount.defaultAccountId = item.Id; 
                        }

                        if (item.CategoryId == 3)
                        {
                            tempStockAccount.Add(new AccountData
                            {
                                Id = item.Id,
                                Name = item.Name,
                                AccountNumber = item.AccountNumber,
                                TaxId = item.TaxId,
                                TaxName = item.TaxName,
                                Description = item.Description,
                                CategoryId = item.CategoryId,
                            });
                        }
                    }

                    result.PurchaseAccount.AccountList = tempPurchaseAccount;
                    result.SalesAccount.AccountList = tempSalesAccount;
                    result.StockAccount.AccountList = tempSalesAccount;

                    response.IsOk = true;
                    response.Row = result;
                    response.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.IsOk = false;
                response.Row = null;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }

    public class AccountPurchaseSalesList : IRequest<RowResponse<AccountPurchaseSalesListDto>>
    {
    }

    public class AccountPurchaseSalesListDto
    { 
        public AccountFetchDataList PurchaseAccount { get; set; }
        public AccountFetchDataList SalesAccount { get; set; }
        public AccountFetchDataList StockAccount { get; set; }
    }
    public class AccountFetchDataList
    {
        public int defaultAccountId { get; set; }
        public List<AccountData> AccountList { get; set; }
    }
    public class AccountData : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public int TaxId { get; set; } = 0;
        public string TaxName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; } = 0;
    }

}
