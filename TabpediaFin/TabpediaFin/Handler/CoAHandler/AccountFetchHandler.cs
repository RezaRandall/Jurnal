namespace TabpediaFin.Handler.CoAHandler
{
    public class AccountFetchHandler : IFetchByIdHandler<AccountCoAFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public AccountFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<AccountCoAFetchDto>> Handle(FetchByIdRequestDto<AccountCoAFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<AccountCoAFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<AccountCoAFetchDto>(request.Id, _currentUser);

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

    [Table("Account")]
    public class AccountCoAFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public int CategoryId { get; set; } = 0;
        public int AccountParentId { get; set; } = 0;
        public int TaxId { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public double Balance { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public int BankId { get; set; } = 0;
    }
}
