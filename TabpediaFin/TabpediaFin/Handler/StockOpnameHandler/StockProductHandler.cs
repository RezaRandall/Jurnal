namespace TabpediaFin.Handler.StockHandler
{
    public class StockListHandler : IFetchPagedListStockItem<StockProductListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public StockListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<PagedListResponse<StockProductListDto>> Handle(FetchPagedListStockItem<StockProductListDto> request, CancellationToken cancellationToken)
        {
            var response = new PagedListResponse<StockProductListDto>();
            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    string stockfilter = "";

                    if (request.WarehouseId != 0 && request.ItemId != 0)
                    {
                        stockfilter = @" and i.""WarehouseId"" = " + request.WarehouseId + @" and i.""ItemId"" = " + request.ItemId + "";
                    }
                    else if (request.WarehouseId != 0 && request.ItemId == 0)
                    {
                        stockfilter = @" and i.""WarehouseId"" = " + request.WarehouseId + "";
                    }
                    else if (request.ItemId != 0 && request.WarehouseId == 0)
                    {
                        stockfilter = @"and i.""ItemId"" = " + request.ItemId + "";
                    }

                    var sql = @"SELECT it.""Name"" as ItemName,i.""ItemId"", w.""Name"" as WarehouseName, i.""WarehouseId"", ""sum""(i.""Quantity"") as Stock FROM ""ItemStock"" i LEFT JOIN ""Warehouse"" w ON i.""WarehouseId"" = w.""Id"" LEFT JOIN ""Item"" it ON i.""ItemId"" = it.""Id"" WHERE i.""TenantId"" = @TenantId " + stockfilter + @" GROUP BY i.""ItemId"", i.""WarehouseId"", w.""Name"",it.""Name""";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);

                    List<StockProductListDto> result;
                    result = (await cn.QueryAsync<StockProductListDto>(sql, parameters).ConfigureAwait(false)).ToList();

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
    

    public class StockProductListDto
    {
        public int WarehouseId { get; set; } = 0;
        public string WarehouseName { get; set; } = string.Empty;
        public int ItemId { get; set; } = 0;
        public string ItemName { get; set; } = string.Empty;
        public double Stock { get; set; } = 0;
    }

    public class FetchPagedListStockItem<T> : IRequest<PagedListResponse<T>>
    {
        public int ItemId { get; set; } = 0;
        public int WarehouseId { get; set; } = 0;
    }
    public interface IFetchPagedListStockItem<T> : IRequestHandler<FetchPagedListStockItem<T>, PagedListResponse<T>>
    {
    }
}
