namespace TabpediaFin.Handler.CoAHandler
{
    public class AccountPurchaseSalesListHandler : IRequestHandler<AccountPurchaseSalesList, PagedListResponse<AccountPurchaseSalesListDto>>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public AccountPurchaseSalesListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<PagedListResponse<AccountPurchaseSalesListDto>> Handle(AccountPurchaseSalesList request, CancellationToken cancellationToken)
        {
            var response = new PagedListResponse<AccountPurchaseSalesListDto>();
            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sql = @"SELECT i.""Name"", i.""AccountNumber"", i.""CategoryId"", i.""AccountParentId"", i.""TaxId"", i.""Description"", i.""Balance"", i.""IsLocked"", i.""BankId""  FROM ""Account"" i LEFT JOIN ""Tax"" t ON i.""TaxId"" = t.""Id"" LEFT JOIN ""AccountCategory"" ac ON i.""CategoryId"" = ac.""Id""  WHERE i.""TenantId"" = @TenantId ";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);

                    List<AccountPurchaseSalesListDto> result;
                    result = (await cn.QueryAsync<AccountPurchaseSalesListDto>(sql, parameters).ConfigureAwait(false)).ToList();

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

    public class AccountPurchaseSalesList : IRequest<PagedListResponse<AccountPurchaseSalesListDto>>
    {
        public string type { get; set; } = string.Empty;
    }

    public class AccountPurchaseSalesListDto
    { 
        public List<AccountData> PurchaseAccountList { get; set; }
        public List<AccountData> SalesAccountList { get; set; }
    }
    public class AccountData : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public int TaxId { get; set; } = 0;
        public string TaxName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

}
