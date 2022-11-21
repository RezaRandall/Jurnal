namespace TabpediaFin.Handler.AddressTypeHandler
{
    public class ItemCategoryFetchHandler : IQueryByIdHandler<AddressTypeFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public ItemCategoryFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<AddressTypeFetchDto>> Handle(QueryByIdDto<AddressTypeFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<AddressTypeFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<AddressTypeFetchDto>(request.Id, _currentUser);

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

    [Table("AddressType")]
    public class AddressTypeFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
