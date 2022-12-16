using TabpediaFin.Handler.ContactAddressHandler;
using TabpediaFin.Handler.ContactPersonHandler;

namespace TabpediaFin.Handler.ContactHandler
{
    public class ContactUpdateHandler : IRequestHandler<ContactUpdateDto, RowResponse<ContactFetchDto>>
    {
        private readonly DbManager _dbManager;
        private readonly FinContext _context;
        private readonly ICurrentUser _currentUser;

        public ContactUpdateHandler(FinContext db, ICurrentUser currentUser, DbManager dbManager)
        {
            _context = db;
            _currentUser = currentUser;
            _dbManager = dbManager;
        }

        public async Task<RowResponse<ContactFetchDto>> Handle(ContactUpdateDto request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<ContactFetchDto>();
            int contactidresult;
            List<ContactAddress> ContactAddress = new List<ContactAddress>();
            List<ContactPerson> ContactPerson = new List<ContactPerson>();
            List<ContactAddressFetchDto> ContactAddressFetchDto = new List<ContactAddressFetchDto>();
            List<ContactPersonFetchDto> ContactPersonFetchDto = new List<ContactPersonFetchDto>();
            try
            {
                var Contact = await _context.Contact.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
                Contact.Name = request.Name;
                Contact.Address = request.Address;
                Contact.CityName = request.CityName;
                Contact.PostalCode = request.PostalCode;
                Contact.Email = request.Email;
                Contact.Phone = request.Phone;
                Contact.Fax = request.Fax;
                Contact.Website = request.Website;
                Contact.Npwp = request.Npwp;
                Contact.GroupId = request.GroupId;
                Contact.IsCustomer = request.IsCustomer;
                Contact.IsVendor = request.IsVendor;
                Contact.IsEmployee = request.IsEmployee;
                Contact.IsOther = request.IsOther;
                Contact.Notes = request.Notes;

                contactidresult = request.Id;
                List<int> idupdateaddtess = new List<int>();
                List<int> idupdateperson = new List<int>();
                if (request.ContactAddressList.Count > 0)
                {
                    foreach (ContactAddressUpdate item in request.ContactAddressList)
                    {
                        idupdateaddtess.Add(item.Id);
                        ContactAddress.Add(new ContactAddress
                        {
                            Id = item.Id,
                            ContactId = contactidresult,
                            AddressName = item.AddressName,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            AddressTypeId = item.AddressTypeId,
                            //AddresType = item.AddresType,
                            Notes = item.Notes,
                            CreatedUid = _currentUser.UserId,
                        });
                        ContactAddressFetchDto.Add(new ContactAddressFetchDto
                        {
                            Id = item.Id,
                            ContactId = contactidresult,
                            AddressName = item.AddressName,
                            Address = item.Address,
                            CityName = item.CityName,
                            PostalCode = item.PostalCode,
                            AddressTypeId = item.AddressTypeId,
                            Notes = item.Notes,
                        });
                    }
                    _context.ContactAddress.UpdateRange(ContactAddress);
                }
                
                if (request.ContactPersonList.Count > 0)
                {
                    foreach (ContactPersonUpdate item in request.ContactPersonList)
                    {
                        idupdateperson.Add(item.Id);
                        ContactPerson.Add(new ContactPerson
                        {
                            Id = item.Id,
                            ContactId = contactidresult,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Others = item.Others,
                            Notes = item.Notes,
                            CreatedUid = _currentUser.UserId,
                        });
                        ContactPersonFetchDto.Add(new ContactPersonFetchDto
                        {
                            Id = item.Id,
                            ContactId = contactidresult,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Others = item.Others,
                            Notes = item.Notes,
                        });
                    }
                    _context.ContactPerson.UpdateRange(ContactPerson);
                }
                List<ContactPerson> ContacPersonList = _context.ContactPerson.Where<ContactPerson>(x => x.ContactId == request.Id && x.TenantId == _currentUser.TenantId && !idupdateperson.Contains(x.Id)).ToList();
                List<ContactAddress> ContactAddressList = _context.ContactAddress.Where<ContactAddress>(x => x.ContactId == request.Id && x.TenantId == _currentUser.TenantId && !idupdateaddtess.Contains(x.Id)).ToList();
                _context.ContactAddress.RemoveRange(ContactAddressList);
                _context.ContactPerson.RemoveRange(ContacPersonList);
                await _context.SaveChangesAsync(cancellationToken);

                var row = new ContactFetchDto()
                {
                    Id = request.Id,
                    Name = Contact.Name,
                    Address = Contact.Address,
                    CityName = Contact.CityName,
                    PostalCode = Contact.PostalCode,
                    Email = Contact.Email,
                    Phone = Contact.Phone,
                    Fax = Contact.Fax,
                    Website = Contact.Website,
                    Npwp = Contact.Npwp,
                    GroupId = Contact.GroupId,
                    IsCustomer = Contact.IsCustomer,
                    IsVendor = Contact.IsVendor,
                    IsEmployee = Contact.IsEmployee,
                    IsOther = Contact.IsOther,
                    Notes = Contact.Notes,
                    ContactAddressList = ContactAddressFetchDto,
                    ContactPersonList = ContactPersonFetchDto,
                };

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = row;
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public async Task<ContactFetchDto> GetContact(int id)
        {
            ContactFetchDto returncontact = new ContactFetchDto();
            using (var cn = _dbManager.CreateConnection())
            {
                var sql = @"SELECT c.""Name"" as groupName, i.""Id"", i.""TenantId"",i.""Name"", i.""Address"", i.""CityName"",i.""PostalCode"",i.""Email"",i.""Phone"",i.""Fax"",i.""Website"",i.""Npwp"",i.""GroupId"",i.""Notes"", i.""IsCustomer"", i.""IsVendor"", i.""IsEmployee"", i.""IsOther""  FROM ""Contact"" i LEFT JOIN ""ContactGroup"" c on i.""GroupId"" = c.""Id""  WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("Id", id);
                var result = await cn.QueryFirstOrDefaultAsync<ContactFetchDto>(sql, parameters);

                if (result != null)
                {
                    var sqladdress = @"SELECT at.""Name"" as AddresType, i.""Id"", i.""ContactId"", i.""AddressName"",i.""Address"",i.""CityName"",i.""PostalCode"",i.""AddressTypeId"",i.""Notes""  FROM ""ContactAddress"" i LEFT JOIN ""ContactAddressType"" at ON i.""AddressTypeId"" = at.""Id"" WHERE i.""TenantId"" = @TenantId AND i.""ContactId"" = @ContactId";

                    var parametersub = new DynamicParameters();
                    parametersub.Add("TenantId", _currentUser.TenantId);
                    parametersub.Add("ContactId", id);

                    List<ContactAddressFetchDto> resultaddress;
                    resultaddress = (await cn.QueryAsync<ContactAddressFetchDto>(sqladdress, parametersub).ConfigureAwait(false)).ToList();

                    result.ContactAddressList = resultaddress;
                    var sqlperson = @"SELECT i.""Id"", i.""ContactId"", i.""Name"",i.""Email"",i.""Phone"",i.""Fax"",i.""Others"",i.""Notes""  FROM ""ContactPerson"" i WHERE i.""TenantId"" = @TenantId AND i.""ContactId"" = @ContactId";

                    List<ContactPersonFetchDto> resultperson;
                    resultperson = (await cn.QueryAsync<ContactPersonFetchDto>(sqlperson, parametersub).ConfigureAwait(false)).ToList();

                    result.ContactPersonList = resultperson;
                }

                
                returncontact = result;
            }


            return returncontact;
        }
    }

    public class IdComparer : IEqualityComparer<ContactAddress>
    {
        public int GetHashCode(ContactAddress co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.Id.GetHashCode();
        }

        public bool Equals(ContactAddress x1, ContactAddress x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.Id == x2.Id;
        }
    }

    public class ContactUpdateDto : IRequest<RowResponse<ContactFetchDto>>
    {
        public int Id { get; set; }
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
        public List<ContactAddressUpdate> ContactAddressList { get; set; }
        public List<ContactPersonUpdate> ContactPersonList { get; set; }

    }

    public class ContactAddressUpdate
    {
        public int Id { get; set; }
        public string AddressName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int AddressTypeId { get; set; } = 0;
        public string AddresType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
    public class ContactPersonUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Others { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
