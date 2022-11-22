namespace TabpediaFin.Handler.Item
{
    public class ItemHandler : IQueryByIdHandler<ItemDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ItemHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<ItemDto>> Handle(QueryByIdDto<ItemDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ItemDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<ItemDto>(request.Id, _currentUser);
                    if (row == null)
                    {
                        response.IsOk = false;
                        response.Row = row;
                        response.ErrorMessage = "Data not found";
                    }
                    else 
                    {
                        response.IsOk = true;
                        response.Row = row;
                        response.ErrorMessage = string.Empty;
                    }
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
}

[Table("Item")]
public class ItemDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public int UnitMeasureId { get; set; } = 0;
    public int AverageCost { get; set; } = 0;
    public int Cost { get; set; } = 0;
    public int Price { get; set; } = 0;
    public bool IsSales { get; set; } = true;
    public bool IsPurchase { get; set; } = true;
    public bool IsStock { get; set; } = true;
    public int StockMin { get; set; } = 0;
    public bool IsArchived { get; set; } = true;
    public string ImageFileName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}