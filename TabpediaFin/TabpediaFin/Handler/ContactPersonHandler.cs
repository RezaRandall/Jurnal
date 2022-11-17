namespace TabpediaFin.Handler
{
    public class ContactPersonFetchHandler : IQueryByIdHandler<ContactPersonDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ContactPersonFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<ContactPersonDto>> Handle(QueryByIdDto<ContactPersonDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ContactPersonDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<ContactPersonDto>(request.Id, _currentUser);

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
