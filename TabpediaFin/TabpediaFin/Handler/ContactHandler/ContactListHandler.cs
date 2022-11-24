namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactListHandler : IQueryPagedListContactHandler<contactlistDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ContactListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<PagedListResponse<contactlistDto>> Handle(QueryPagedListContactDto<contactlistDto> request, CancellationToken cancellationToken)
        {
            if (request.PageNum == 0) { request.PageNum = 1; }
            if (request.PageSize == 0) { request.PageSize = 10; }
            var response = new PagedListResponse<contactlistDto>();
            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    string sqlsort = "";
                    string sqlsearch = "";
                    string contactfilter = "";

                    if (request.contacttype == "customer")
                    {
                        contactfilter = @"and ""IsCustomer"" = true";
                    }
                    else if (request.contacttype == "vendor")
                    {
                        contactfilter = @"and ""IsVendor""= true";
                    }
                    else if (request.contacttype == "employee")
                    {
                        contactfilter = @"and ""IsEmployee""= true";
                    }
                    else if (request.contacttype == "other")
                    {
                        contactfilter = @"and ""IsOther"" = true";
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
                        sqlsearch = @"AND LOWER(i.""Name"") LIKE @Search  AND LOWER(i.""Address"") LIKE @Search  AND LOWER(i.""CityName"") LIKE @Search  AND LOWER(i.""PostalCode"") LIKE @Search  AND LOWER(i.""Email"") LIKE @Search  AND LOWER(i.""Phone"") LIKE @Search  AND LOWER(i.""Fax"") LIKE @Search  AND LOWER(i.""Website"") LIKE @Search  AND LOWER(i.""Npwp"") LIKE @Search";
                    }

                    var sql = @"SELECT c.""Name"" as groupName, i.""Id"", i.""TenantId"",i.""Name"", i.""Address"", i.""CityName"",i.""PostalCode"",i.""Email"",i.""Phone"",i.""Fax"",i.""Website"",i.""Npwp"",i.""GroupId"",i.""Notes"", i.""IsCustomer"", i.""IsVendor"", i.""IsEmployee"", i.""IsOther""  FROM ""Contact"" i LEFT JOIN ""ContactGroup"" c on i.""GroupId"" = c.""Id"" WHERE i.""TenantId"" = @TenantId " + contactfilter + " " + sqlsearch + " " + sqlsort + " LIMIT @PageSize OFFSET @PageNum";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("PageSize", request.PageSize);
                    parameters.Add("PageNum", request.PageNum);
                    parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                    List<contactlistDto> result;
                    result = (await cn.QueryAsync<contactlistDto>(sql, parameters).ConfigureAwait(false)).ToList();

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

    [Table("Contact")]
    public class contactlistDto : BaseDto
    {
        [Searchable]
        public string Name { get; set; } = string.Empty;
        [Searchable]
        public string Address { get; set; } = string.Empty;
        [Searchable]
        public string CityName { get; set; } = string.Empty;
        [Searchable]
        public string PostalCode { get; set; } = string.Empty;
        [Searchable]
        public string Email { get; set; } = string.Empty;
        [Searchable]
        public string Phone { get; set; } = string.Empty;
        [Searchable]
        public string Fax { get; set; } = string.Empty;
        [Searchable]
        public string Website { get; set; } = string.Empty;
        [Searchable]
        public string Npwp { get; set; } = string.Empty;
        [Searchable]
        public string groupName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsOther { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class QueryPagedListContactDto<T> : IRequest<PagedListResponse<T>>
    {
        public int PageSize { get; set; } = 10;

        public int PageNum { get; set; } = 1;

        public string Search { get; set; } = string.Empty;

        public string SortBy { get; set; } = string.Empty;

        public bool SortDesc { get; set; }
        public string contacttype { get; set; } = string.Empty;
    }

    public interface IQueryPagedListContactHandler<T> : IRequestHandler<QueryPagedListContactDto<T>, PagedListResponse<T>>
    {
    }
}
