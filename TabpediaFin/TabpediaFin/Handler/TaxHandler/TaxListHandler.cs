﻿namespace TabpediaFin.Handler.TaxHandler
{
    public class AddressTypeListHandler : IFetchPagedListHandler<TaxListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public AddressTypeListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<TaxListDto>> Handle(FetchPagedListRequestDto<TaxListDto> request, CancellationToken cancellationToken)
        {
            if (request.PageNum == 0) { request.PageNum = 1; }
            if (request.PageSize == 0) { request.PageSize = 10; }

            var result = new PagedListResponse<TaxListDto>();

            try
            {
                string sqlWhere = " WHERE (1=1) ";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    sqlWhere += SqlHelper.GenerateWhere<TaxListDto>();
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

                    var list = await cn.FetchListPagedAsync<TaxListDto>(pageNumber: request.PageNum
                        , rowsPerPage: request.PageSize
                        , conditions: sqlWhere
                        , orderby: orderby
                        , currentUser: _currentUser
                        , parameters: parameters);

                    int recordCount = await cn.RecordCountAsync<TaxListDto>(sqlWhere, parameters);

                    result.IsOk = true;
                    result.ErrorMessage = string.Empty;
                    result.List = list?.AsList() ?? new List<TaxListDto>();
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
    [Table("Tax")]
    public class TaxListDto : BaseDto
    {
        [Searchable]
        public string Name { get; set; } = string.Empty;

        [Searchable]
        public string Description { get; set; } = string.Empty;

        [Searchable]
        public double RatePercent { get; set; }
    }
}
