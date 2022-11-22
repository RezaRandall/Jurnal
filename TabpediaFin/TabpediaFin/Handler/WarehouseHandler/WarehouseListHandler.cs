namespace TabpediaFin.Handler.WarehouseHandler
{
    public class WarehouseListHandler : IQueryPagedListHandler<WarehouseListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public WarehouseListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<WarehouseListDto>> Handle(QueryPagedListDto<WarehouseListDto> request, CancellationToken cancellationToken)
        {
            if (request.PageNum == 0) { request.PageNum = 1; }
            if (request.PageSize == 0) { request.PageSize = 10; }

            var result = new PagedListResponse<WarehouseListDto>();

            try
            {
                string sqlWhere = " WHERE (1=1) ";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    sqlWhere += SqlHelper.GenerateWhere<WarehouseListDto>();
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

                    var list = await cn.FetchListPagedAsync<WarehouseListDto>(pageNumber: request.PageNum
                        , rowsPerPage: request.PageSize
                        , conditions: sqlWhere
                        , orderby: orderby
                        , currentUser: _currentUser
                        , parameters: parameters);

                    int recordCount = await cn.RecordCountAsync<WarehouseListDto>(sqlWhere, parameters);

                    result.IsOk = true;
                    result.ErrorMessage = string.Empty;
                    result.List = list?.AsList() ?? new List<WarehouseListDto>();
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
    [Table("Warehouse")]
    public class WarehouseListDto : BaseDto
    {
        [Searchable]
        public string Name { get; set; } = string.Empty;

        [Searchable]
        public string Description { get; set; } = string.Empty;
        
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
        public string ContactPersonName { get; set; } = string.Empty;
    }
}
