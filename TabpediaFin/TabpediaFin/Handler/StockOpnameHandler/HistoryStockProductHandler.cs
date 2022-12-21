namespace TabpediaFin.Handler.StockHandler
{
    public class HistoryStockListHandler : IFetchByIdHandler<HistoryStockDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public HistoryStockListHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<HistoryStockDto>> Handle(FetchByIdRequestDto<HistoryStockDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<HistoryStockDto>();
            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sql = @"SELECT i.""ItemId"", ii.""Name"", COALESCE(Round((""sum""(Case WHEN i.""Description"" = 'Pembelian' Then (i.""Cost"") end)/""sum""(Case WHEN i.""Description"" = 'Pembelian' Then (i.""Quantity"") end)),2),0) as AverageCost, 
                                COALESCE(Round((""sum""(Case WHEN i.""Description"" = 'Penjualan' Then(i.""Cost"") end) / ""sum""(Case WHEN i.""Description"" = 'Penjualan' Then(i.""Quantity"") end)), 2), 0) as AveragePrice,
                                ""max""(Case WHEN i.""Description"" = 'Pembelian' Then i.""CreatedUtc"" end) as LastPurchase,
                                ""max""(Case WHEN i.""Description"" = 'Penjualan' Then i.""CreatedUtc"" end) as LastSales
                                FROM ""ItemStock"" i
                                LEFT JOIN ""Item"" ii ON i.""ItemId"" = ii.""Id""
                                where i.""TenantId"" = @TenantId and i.""ItemId"" = @ItemId 
                                group by i.""ItemId"",ii.""Name"" ";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("ItemId", request.Id);
                    
                    
                    var result = await cn.QueryFirstOrDefaultAsync<HistoryStockDto>(sql, parameters);
                    var sqlhistory = @"SELECT w.""Name"" as WarehouseName, i.""WarehouseId"", i.""Quantity"", i.""Description"", i.""Cost"", i.""Price"" FROM ""ItemStock"" i LEFT JOIN ""Warehouse"" w ON i.""WarehouseId"" = w.""Id""  WHERE i.""TenantId"" = @TenantId ";

                    List<ListStock> resulthistory;
                    resulthistory = (await cn.QueryAsync<ListStock>(sqlhistory, parameters).ConfigureAwait(false)).ToList();

                    result.ItemStockList = resulthistory;
                    response.IsOk = true;
                    response.Row = result;
                    response.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.IsOk = false;
                response.Row = null;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
    

    public class HistoryStockDto
    {
        public int Itemid { get; set; } = 0;
        public string ItemName { get; set; } = string.Empty;
        public double AverageCost { get; set; } = 0;
        public double AveragePrice { get; set; } = 0;
        public DateTime LastPurchase { get; set; }
        public DateTime lastSales { get; set; }
        public List<ListStock> ItemStockList;
    }

    public class ListStock
    {
        public int WarehouseId { get; set; } = 0;
        public string WarehouseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Quantity { get; set; } = 0;
        public double Cost { get; set; } = 0;
        public double Price { get; set; } = 0;
    }
}
