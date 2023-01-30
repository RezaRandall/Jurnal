namespace TabpediaFin.Handler.CoAHandler
{
    public class GenerateNumberHandler : IFetchByIdHandler<GenerateNumberDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public GenerateNumberHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<GenerateNumberDto>> Handle(FetchByIdRequestDto<GenerateNumberDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<GenerateNumberDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sql = @"SELECT ""AccountNumber"" From ""Account"" WHERE ""CategoryId"" = @CategoryId and ""TenantId"" = @TenantId order by ""AccountNumber"" desc";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("CategoryId", request.Id);

                    var result = await cn.QueryFirstOrDefaultAsync<GenerateNumberDto>(sql, parameters);
                    int temp = Int16.Parse(result.AccountNumber.Split('-').Last());
                    temp++;
                    result.AccountNumber = result.AccountNumber.Split('-').First() + "-" + temp.ToString();

                    response.IsOk = true;
                    response.Row = result;
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
    public class GenerateNumberDto
    {
        public string AccountNumber { get; set; } = string.Empty;
    }
}
