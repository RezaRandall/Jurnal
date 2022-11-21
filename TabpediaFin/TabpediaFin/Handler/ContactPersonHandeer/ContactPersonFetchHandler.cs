namespace TabpediaFin.Handler.ContactPersonHandler
{
    public class ContactPersonFetchHandler : IQueryByIdHandler<ContactPersonFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public ContactPersonFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<ContactPersonFetchDto>> Handle(QueryByIdDto<ContactPersonFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ContactPersonFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<ContactPersonFetchDto>(request.Id, _currentUser);

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

    [Table("ContactPerson")]
    public class ContactPersonFetchDto : BaseDto
    {
        public int ContactId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Others { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
