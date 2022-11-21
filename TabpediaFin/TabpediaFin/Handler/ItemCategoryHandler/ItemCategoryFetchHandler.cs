namespace TabpediaFin.Handler.ItemCategoryHandler
{
    public class ItemCategoryFetchHandler : IQueryByIdHandler<ItemCategoryFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ItemCategoryFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<ItemCategoryFetchDto>> Handle(QueryByIdDto<ItemCategoryFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ItemCategoryFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<ItemCategoryFetchDto>(request.Id, _currentUser);

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

    [Table("ItemCategory")]
    public class ItemCategoryFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
