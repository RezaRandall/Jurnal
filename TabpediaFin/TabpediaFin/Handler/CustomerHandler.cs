//using TabpediaFin.Dto.Common.Request;
//using TabpediaFin.Handler.ContactAddressHandler;
//using TabpediaFin.Handler.ContactPersonHandler;
//using TabpediaFin.Handler.Interfaces;

//namespace TabpediaFin.Handler
//{
//    public class GetListCustomerHandler : IRequestHandler<GetCustomerListQuery, List<Contact>>
//    {
//        private readonly ICustomerRepository _customerRepository;
//        public GetListCustomerHandler(ICustomerRepository customerRepository)
//        {
//            _customerRepository = customerRepository;
//        }

//        public async Task<List<Contact>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
//        {
//            var result = await _customerRepository.GetCustomerList(request);
//            return result;
//        }
//    }

//    public class ContactFetchHandler : IQueryByIdHandler<ContactDto>
//    {
//        private readonly DbManager _dbManager;
//        private readonly ICurrentUser _currentUser;

//        public ContactFetchHandler(DbManager dbManager, ICurrentUser currentUser)
//        {
//            _dbManager = dbManager;
//            _currentUser = currentUser;
//        }

//        public async Task<RowResponse<ContactDto>> Handle(QueryByIdDto<ContactDto> request, CancellationToken cancellationToken)
//        {
//            var response = new RowResponse<ContactDto>();

//            try
//            {
//                using (var cn = _dbManager.CreateConnection())
//                {
//                    var sql = @"SELECT c.""Name"" as groupName, i.""Id"", i.""TenantId"",i.""Name"", i.""Address"", i.""CityName"",i.""PostalCode"",i.""Email"",i.""Phone"",i.""Fax"",i.""Website"",i.""Npwp"",i.""GroupId"",i.""Notes"", i.""IsCustomer"", i.""IsVendor"", i.""IsEmployee"", i.""IsOther""  FROM ""Contact"" i LEFT JOIN ""ContactGroup"" c on i.""GroupId"" = c.""Id""  WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @Id";

//                    var parameters = new DynamicParameters();
//                    parameters.Add("TenantId", _currentUser.TenantId);
//                    parameters.Add("Id", request.Id);
//                    var result = await cn.QueryFirstOrDefaultAsync<ContactDto>(sql, parameters);

//                    if(result != null)
//                    {
//                        var sqladdress = @"SELECT at.""Name"" as AddresType, i.""Id"", i.""ContactId"", i.""AddressName"",i.""Address"",i.""CityName"",i.""PostalCode"",i.""AddressTypeId"",i.""Notes""  FROM ""ContactAddress"" i LEFT JOIN ""AddressType"" at ON i.""AddressTypeId"" = at.""Id"" WHERE i.""TenantId"" = @TenantId AND i.""ContactId"" = @ContactId";

//                        var parametersub = new DynamicParameters();
//                        parametersub.Add("TenantId", _currentUser.TenantId);
//                        parametersub.Add("ContactId", request.Id);

//                        List<ContactAddressFetchDto> resultaddress;
//                        resultaddress = (await cn.QueryAsync<ContactAddressFetchDto>(sqladdress, parametersub).ConfigureAwait(false)).ToList();

//                        result.ContactAddressList = resultaddress;
//                        var sqlperson = @"SELECT i.""Id"", i.""ContactId"", i.""Name"",i.""Email"",i.""Phone"",i.""Fax"",i.""Others"",i.""Notes""  FROM ""ContactPerson"" i WHERE i.""TenantId"" = @TenantId AND i.""ContactId"" = @ContactId";

//                        List<ContactPersonFetchDto> resultperson;
//                        resultperson = (await cn.QueryAsync<ContactPersonFetchDto>(sqlperson, parametersub).ConfigureAwait(false)).ToList();

//                        result.ContactPersonList = resultperson;
//                    }

//                    response.IsOk = true;
//                    response.Row = result;
//                    response.ErrorMessage = string.Empty;
//                }
//            }
//            catch (Exception ex)
//            {
//                response.IsOk = false;
//                response.Row = null;
//                response.ErrorMessage = ex.Message;
//            }

//            return response;
//        }
//    }

//    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomer, bool>
//    {
//        private readonly ICustomerRepository _customerRepository;
//        public DeleteCustomerHandler(ICustomerRepository customerRepository)
//        {
//            _customerRepository = customerRepository;
//        }

//        public async Task<bool> Handle(DeleteCustomer request, CancellationToken cancellationToken)
//        {
//            var result = await _customerRepository.DeleteCustomer(request);
//            return result;
//        }
//    }
//    public class AddCustomerHandler : IRequestHandler<AddCustomer, Contact>
//    {
//        private readonly ICustomerRepository _customerRepository;
//        public AddCustomerHandler(ICustomerRepository customerRepository)
//        {
//            _customerRepository = customerRepository;
//        }

//        async Task<Contact> IRequestHandler<AddCustomer, Contact>.Handle(AddCustomer request, CancellationToken cancellationToken)
//        {
//            var result = await _customerRepository.CreateCustomer(request);
//            return result;
//        }
//    }
//    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomer, Contact>
//    {
//        private readonly ICustomerRepository _customerRepository;
//        public UpdateCustomerHandler(ICustomerRepository customerRepository)
//        {
//            _customerRepository = customerRepository;
//        }

//        async Task<Contact> IRequestHandler<UpdateCustomer, Contact>.Handle(UpdateCustomer request, CancellationToken cancellationToken)
//        {
//            var result = await _customerRepository.UpdateCustomer(request);
//            return result;
//        }
//    }
//}