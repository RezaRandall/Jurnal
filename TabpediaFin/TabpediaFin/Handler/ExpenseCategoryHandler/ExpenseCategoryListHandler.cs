namespace TabpediaFin.Handler.ExpenseCategoryHandler
{
    public class ExpenseCategoryListHandler : IFetchPagedListHandler<ExpenseCategoryListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ExpenseCategoryListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<ExpenseCategoryListDto>> Handle(FetchPagedListRequestDto<ExpenseCategoryListDto> request, CancellationToken cancellationToken)
        {
            if (request.PageNum == 0) { request.PageNum = 1; }
            if (request.PageSize == 0) { request.PageSize = 10; }

            var result = new PagedListResponse<ExpenseCategoryListDto>();

            try
            {
                string sqlWhere = " WHERE (1=1) ";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    sqlWhere += SqlHelper.GenerateWhere<ExpenseCategoryListDto>();
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

                    var list = await cn.FetchListPagedAsync<ExpenseCategoryListDto>(pageNumber: request.PageNum
                        , rowsPerPage: request.PageSize
                        , conditions: sqlWhere
                        , orderby: orderby
                        , currentUser: _currentUser
                        , parameters: parameters);

                    int recordCount = await cn.RecordCountAsync<ExpenseCategoryListDto>(sqlWhere, parameters);

                    result.IsOk = true;
                    result.ErrorMessage = string.Empty;
                    result.List = list?.AsList() ?? new List<ExpenseCategoryListDto>();
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
    [Table("ExpenseCategory")]
    public class ExpenseCategoryListDto : BaseDto
    {
        [Searchable]
        public string Name { get; set; } = string.Empty;

        [Searchable]
        public string Description { get; set; } = string.Empty;

        [Searchable]
        public int AccountId { get; set; }
    }
}
