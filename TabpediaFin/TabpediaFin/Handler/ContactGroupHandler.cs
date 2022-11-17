namespace TabpediaFin.Handler
{
    public class ContactGroupFetchHandler : IQueryByIdHandler<ContactGroupDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ContactGroupFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<ContactGroupDto>> Handle(QueryByIdDto<ContactGroupDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ContactGroupDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<ContactGroupDto>(request.Id, _currentUser);

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
}
