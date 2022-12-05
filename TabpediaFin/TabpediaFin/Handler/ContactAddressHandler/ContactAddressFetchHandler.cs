namespace TabpediaFin.Handler.ContactAddressHandler
{
    public class ContactAddressFetchHandler : IFetchByIdHandler<ContactAddressFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public ContactAddressFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<ContactAddressFetchDto>> Handle(FetchByIdRequestDto<ContactAddressFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ContactAddressFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sql = @"SELECT i.*, c.""Name"" as AddressType FROM ""ContactAddress"" i LEFT JOIN ""AddressType"" c on i.""AddressTypeId"" = c.""Id""  WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @Id";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("Id", request.Id);
                    var result = await cn.QueryFirstOrDefaultAsync<ContactAddressFetchDto>(sql, parameters);

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

    [Table("ContactAddress")]
    public class ContactAddressFetchDto : BaseDto
    {
        public int ContactId { get; set; } = 0;
        public string AddressName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int AddressTypeId { get; set; } = 0;
        public string AddresType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
