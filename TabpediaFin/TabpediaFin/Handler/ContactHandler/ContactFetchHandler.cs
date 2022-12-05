using TabpediaFin.Handler.ContactAddressHandler;
using TabpediaFin.Handler.ContactPersonHandler;

namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactFetchHandler : IFetchByIdHandler<ContactFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ContactFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<ContactFetchDto>> Handle(FetchByIdRequestDto<ContactFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ContactFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sql = @"SELECT c.""Name"" as groupName, i.""Id"", i.""TenantId"",i.""Name"", i.""Address"", i.""CityName"",i.""PostalCode"",i.""Email"",i.""Phone"",i.""Fax"",i.""Website"",i.""Npwp"",i.""GroupId"",i.""Notes"", i.""IsCustomer"", i.""IsVendor"", i.""IsEmployee"", i.""IsOther""  FROM ""Contact"" i LEFT JOIN ""ContactGroup"" c on i.""GroupId"" = c.""Id""  WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @Id";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("Id", request.Id);
                    var result = await cn.QueryFirstOrDefaultAsync<ContactFetchDto>(sql, parameters);

                    if (result != null)
                    {
                        var sqladdress = @"SELECT at.""Name"" as AddresType, i.""Id"", i.""ContactId"", i.""AddressName"",i.""Address"",i.""CityName"",i.""PostalCode"",i.""AddressTypeId"",i.""Notes""  FROM ""ContactAddress"" i LEFT JOIN ""ContactAddressType"" at ON i.""AddressTypeId"" = at.""Id"" WHERE i.""TenantId"" = @TenantId AND i.""ContactId"" = @ContactId";

                        var parametersub = new DynamicParameters();
                        parametersub.Add("TenantId", _currentUser.TenantId);
                        parametersub.Add("ContactId", request.Id);

                        List<ContactAddressFetchDto> resultaddress;
                        resultaddress = (await cn.QueryAsync<ContactAddressFetchDto>(sqladdress, parametersub).ConfigureAwait(false)).ToList();

                        result.ContactAddressList = resultaddress;
                        var sqlperson = @"SELECT i.""Id"", i.""ContactId"", i.""Name"",i.""Email"",i.""Phone"",i.""Fax"",i.""Others"",i.""Notes""  FROM ""ContactPerson"" i WHERE i.""TenantId"" = @TenantId AND i.""ContactId"" = @ContactId";

                        List<ContactPersonFetchDto> resultperson;
                        resultperson = (await cn.QueryAsync<ContactPersonFetchDto>(sqlperson, parametersub).ConfigureAwait(false)).ToList();

                        result.ContactPersonList = resultperson;
                    }

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

    [Table("Contact")]
    public class ContactFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Npwp { get; set; } = string.Empty;
        public string groupName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsOther { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<ContactAddressFetchDto> ContactAddressList { get; set; }
        public List<ContactPersonFetchDto> ContactPersonList { get; set; }
    }
}
