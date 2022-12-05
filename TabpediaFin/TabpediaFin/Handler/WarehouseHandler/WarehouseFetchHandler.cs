namespace TabpediaFin.Handler.WarehouseHandler
{
    public class WarehouseFetchHandler : IFetchByIdHandler<WarehouseFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public WarehouseFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<WarehouseFetchDto>> Handle(FetchByIdRequestDto<WarehouseFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<WarehouseFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<WarehouseFetchDto>(request.Id, _currentUser);

                    response.IsOk = true;
                    response.Row = row;
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

    [Table("Warehouse")]
    public class WarehouseFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string ContactPersonName { get; set; } = string.Empty;
    }
}
