namespace TabpediaFin.Handler.ContactAddressTypeHandler
{
    public class ItemCategoryFetchHandler : IQueryByIdHandler<ContactAddressTypeFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public ItemCategoryFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<ContactAddressTypeFetchDto>> Handle(QueryByIdDto<ContactAddressTypeFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ContactAddressTypeFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<ContactAddressTypeFetchDto>(request.Id, _currentUser);

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

    [Table("ContactAddressType")]
    public class ContactAddressTypeFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
