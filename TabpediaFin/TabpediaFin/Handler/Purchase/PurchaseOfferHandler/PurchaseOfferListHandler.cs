namespace TabpediaFin.Handler.PurchaseOfferHandler
{
    public class PurchaseOfferListHandler : IFetchPagedListHandler<PurchaseOfferListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public PurchaseOfferListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<PagedListResponse<PurchaseOfferListDto>> Handle(FetchPagedListRequestDto<PurchaseOfferListDto> request, CancellationToken cancellationToken)
        {
            if (request.PageNum == 0) { request.PageNum = 0; }
            if (request.PageSize == 0) { request.PageSize = 10; }
            var response = new PagedListResponse<PurchaseOfferListDto>();
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

                    var sql = @"SELECT  c.""Name"" as Vendor, 
                                        a.""FullName"" as Staff, 
                                        i.""Id"", 
                                        i.""TenantId"",
                                        i.""StaffId"", 
                                        i.""VendorId"",
                                        i.""TransCode"", 
                                        i.""TransDate"",
                                        i.""DueDate"",
                                        CASE 
                                            WHEN i.""DueDate"" < now()  AND i.""Status"" = 0
                                                THEN 3
                                            ELSE
                                                i.""Status""
                                            END AS Status,
                                        CASE 
                                            WHEN i.""DueDate"" >= now()  AND i.""Status"" = 0
                                                THEN 'Open'
                                            WHEN i.""DueDate"" < now()  AND i.""Status"" = 0
                                                THEN 'Expired'
                                            ELSE
                                                'Closed'
                                            END AS StatusString,
                                        i.""Memo"",
                                        i.""Notes""  
                                        FROM ""PurchaseOffer"" i LEFT JOIN ""AppUser"" a ON a.""Id"" = i.""StaffId"" LEFT JOIN ""Contact"" c ON c.""Id"" = i.""VendorId""  WHERE i.""TenantId"" = @TenantId " + contactfilter + " " + sqlsearch + " " + sqlsort + " LIMIT @PageSize OFFSET @PageNum";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("PageSize", request.PageSize);
                    parameters.Add("PageNum", request.PageNum);
                    parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                    List<PurchaseOfferListDto> result;
                    result = (await cn.QueryAsync<PurchaseOfferListDto>(sql, parameters).ConfigureAwait(false)).ToList();

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
    [Table("PurchaseOffer")]
    public class PurchaseOfferListDto : BaseDto
    {
        [Searchable]
        public int StaffId { get; set; }
        [Searchable]
        public string Staff { get; set; }
        [Searchable]
        public int VendorId { get; set; }
        [Searchable]
        public string Vendor { get; set; }
        [Searchable]
        public DateTime TransDate { get; set; }
        [Searchable]
        public DateTime DueDate { get; set; }
        [Searchable]
        public string TransCode { get; set; } = string.Empty;
        [Searchable]
        public string Memo { get; set; } = string.Empty;
        [Searchable]
        public string Notes { get; set; } = string.Empty;
        [Searchable]
        public int Status { get; set; }
        public string StatusString { get; set; } = string.Empty;
    }

}
