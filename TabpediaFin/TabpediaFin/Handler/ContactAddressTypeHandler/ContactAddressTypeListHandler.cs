namespace TabpediaFin.Handler.ContactAddressTypeHandler
{
    public class ContactAddressTypeListHandler : IQueryPagedListHandler<ContactAddressTypeListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ContactAddressTypeListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<ContactAddressTypeListDto>> Handle(QueryPagedListDto<ContactAddressTypeListDto> request, CancellationToken cancellationToken)
        {
            if (request.PageNum == 0) { request.PageNum = 1; }
            if (request.PageSize == 0) { request.PageSize = 10; }

            var result = new PagedListResponse<ContactAddressTypeListDto>();

            try
            {
                string sqlWhere = " WHERE (1=1) ";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    sqlWhere += SqlHelper.GenerateWhere<ContactAddressTypeListDto>();
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

                    var list = await cn.FetchListPagedAsync<ContactAddressTypeListDto>(pageNumber: request.PageNum
                        , rowsPerPage: request.PageSize
                        , conditions: sqlWhere
                        , orderby: orderby
                        , currentUser: _currentUser
                        , parameters: parameters);

                    int recordCount = await cn.RecordCountAsync<ContactAddressTypeListDto>(sqlWhere, parameters);

                    result.IsOk = true;
                    result.ErrorMessage = string.Empty;
                    result.List = list?.AsList() ?? new List<ContactAddressTypeListDto>();
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
    [Table("ContactAddressType")]
    public class ContactAddressTypeListDto : BaseDto
    {
        [Searchable]
        public string Name { get; set; } = string.Empty;

        [Searchable]
        public string Description { get; set; } = string.Empty;
    }
}
