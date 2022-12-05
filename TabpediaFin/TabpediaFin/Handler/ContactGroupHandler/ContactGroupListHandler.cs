namespace TabpediaFin.Handler.ContactGroupHandler
{
    public class ContactGroupListHandler : IFetchPagedListHandler<ContactGroupListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ContactGroupListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<ContactGroupListDto>> Handle(FetchPagedListRequestDto<ContactGroupListDto> request, CancellationToken cancellationToken)
        {
            if (request.PageNum == 0) { request.PageNum = 1; }
            if (request.PageSize == 0) { request.PageSize = 10; }

            var result = new PagedListResponse<ContactGroupListDto>();

            try
            {
                string sqlWhere = " WHERE (1=1) ";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    sqlWhere += SqlHelper.GenerateWhere<ContactGroupListDto>();
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

                    var list = await cn.FetchListPagedAsync<ContactGroupListDto>(pageNumber: request.PageNum
                        , rowsPerPage: request.PageSize
                        , conditions: sqlWhere
                        , orderby: orderby
                        , currentUser: _currentUser
                        , parameters: parameters);

                    int recordCount = await cn.RecordCountAsync<ContactGroupListDto>(sqlWhere, parameters);

                    result.IsOk = true;
                    result.ErrorMessage = string.Empty;
                    result.List = list?.AsList() ?? new List<ContactGroupListDto>();
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
    [Table("ContactGroup")]
    public class ContactGroupListDto : BaseDto
    {
        [Searchable]
        public string Name { get; set; } = string.Empty;

        [Searchable]
        public string Description { get; set; } = string.Empty;
    }
}
