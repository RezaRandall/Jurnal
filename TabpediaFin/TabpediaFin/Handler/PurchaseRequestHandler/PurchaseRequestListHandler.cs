﻿namespace TabpediaFin.Handler.PurchaseRequestHandler
{
    public class PurchaseRequestListHandler : IFetchPagedListHandler<PurchaseRequestListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public PurchaseRequestListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<PagedListResponse<PurchaseRequestListDto>> Handle(FetchPagedListRequestDto<PurchaseRequestListDto> request, CancellationToken cancellationToken)
        {
            if (request.PageNum == 0) { request.PageNum = 1; }
            if (request.PageSize == 0) { request.PageSize = 10; }
            var response = new PagedListResponse<PurchaseRequestListDto>();
            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    string sqlsort = "";
                    string sqlsearch = "";
                    string contactfilter = "";

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
                        sqlsearch = @"AND LOWER(i.""StaffId"") LIKE @Search  AND LOWER(i.""VendorId"") LIKE @Search  AND LOWER(i.""TransDate"") LIKE @Search  AND LOWER(i.""DueDate"") LIKE @Search  AND LOWER(i.""TransCode"") LIKE @Search  AND LOWER(i.""BudgetYear"") LIKE @Search  AND LOWER(i.""UrgentLevel"") LIKE @Search  AND LOWER(i.""Memo"") LIKE @Search  AND LOWER(i.""Notes"") LIKE @Search";
                    }

                    var sql = @"SELECT i.""Id"", i.""TenantId"",i.""StaffId"", i.""VendorId"", i.""TransDate"",i.""DueDate"",i.""BudgetYear"",i.""UrgentLevel"",i.""Status"",i.""Memo"",i.""Notes""  FROM ""PurchaseRequest"" i WHERE i.""TenantId"" = @TenantId " + contactfilter + " " + sqlsearch + " " + sqlsort + " LIMIT @PageSize OFFSET @PageNum";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("PageSize", request.PageSize);
                    parameters.Add("PageNum", request.PageNum);
                    parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                    List<PurchaseRequestListDto> result;
                    result = (await cn.QueryAsync<PurchaseRequestListDto>(sql, parameters).ConfigureAwait(false)).ToList();

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
    [Table("PurchaseRequest")]
    public class PurchaseRequestListDto : BaseDto
    {
        [Searchable]
        public int StaffId { get; set; }
        [Searchable]
        public int VendorId { get; set; }
        [Searchable]
        public DateTime TransDate { get; set; }
        [Searchable]
        public DateTime DueDate { get; set; }
        [Searchable]
        public string TransCode { get; set; } = string.Empty;
        [Searchable]
        public string BudgetYear { get; set; } = string.Empty;
        [Searchable]
        public int UrgentLevel { get; set; }
        [Searchable]
        public string Memo { get; set; } = string.Empty;
        [Searchable]
        public string Notes { get; set; } = string.Empty;
    }

}
