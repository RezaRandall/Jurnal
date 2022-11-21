namespace TabpediaFin.Handler.ContactGroupHandler
{
    public class AddressTypeFetchHandler : IQueryByIdHandler<ContactGroupFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public AddressTypeFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<ContactGroupFetchDto>> Handle(QueryByIdDto<ContactGroupFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ContactGroupFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<ContactGroupFetchDto>(request.Id, _currentUser);

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

    [Table("ContactGroup")]
    public class ContactGroupFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
