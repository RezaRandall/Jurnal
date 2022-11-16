namespace TabpediaFin.Repository
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(DbManager dbContext) : base(dbContext)
        {

        }

        public async Task<Contact> CreateCustomer(AddCustomer customer)
        {
            var sql = @"INSERT INTO ""Contact"" (""TenantId"",""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid"",""CreatedUtc"", ""IsCustomer"")
            VALUES(@TenantId, @Name, @Address, @CityName,@PostalCode,@Email,@Phone,@Fax,@Website,@Npwp,@GroupId,@Notes,@CreatedUid,@CreatedUtc,TRUE) RETURNING ""Id""";

            //var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", customer.TenantId);
            parameters.Add("Name", customer.Name);
            parameters.Add("Address", customer.Address);
            parameters.Add("CityName", customer.CityName);
            parameters.Add("PostalCode", customer.PostalCode);
            parameters.Add("Email", customer.Email);
            parameters.Add("Phone", customer.Phone);
            parameters.Add("Fax", customer.Fax);
            parameters.Add("Website", customer.Website);
            parameters.Add("Npwp", customer.Npwp);
            parameters.Add("GroupId", customer.GroupId);
            parameters.Add("Notes", customer.Notes);
            parameters.Add("CreatedUid", customer.CreatedUid);
            parameters.Add("CreatedUtc", DateTime.UtcNow);

            using (var cn = _dbManager.CreateConnection())
            {
                int affected = await cn.QueryFirstOrDefaultAsync<int>(sql, parameters);
                Contact resultquery = new Contact();
                GetCustomerQuery param = new GetCustomerQuery();
                param.TenantId = customer.TenantId;
                param.Id = affected;
                resultquery = await GetCustomer(param);

                return resultquery;
            }
        }

        public async Task<bool> DeleteCustomer(DeleteCustomer request)
        {
            var sql = @"DELETE FROM ""Contact"" where IsCustomer = true, ""Id"" = @Id and ""TenantId"" = @TenantId";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("Id", request.Id);
            parameters.Add("TenantId", request.TenantId);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }

        public async Task<Contact> GetCustomer(GetCustomerQuery request)
        {
            var sql = @"SELECT ""Id"", ""TenantId"",""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid""  FROM ""Contact"" WHERE ""TenantId"" = @TenantId AND ""IsCustomer"" = TRUE AND ""Id"" = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", request.TenantId);
            parameters.Add("Id", request.Id);

            using (var cn = _dbManager.CreateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<Contact>(sql, parameters);
            }
        }

        public async Task<List<Contact>> GetCustomerList(GetCustomerListQuery request)
        {
            string sqlsort = "";
            string sqlsearch = "";
            if (request.sortby != null && request.valsort != null && request.sortby != "" && request.valsort != "")
            {
                sqlsort = @" order by """ + request.sortby + "\" " + request.valsort + "";
            }
            if (request.searchby != null && request.valsearch != null && request.searchby != "" && request.valsearch != "")
            {
                sqlsearch = @"AND """ + request.searchby + "\" = '" + request.valsearch + "'";
            }

            var sql = @"SELECT ""Id"", ""TenantId"", ""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid""  FROM ""Contact"" WHERE ""TenantId"" = @TenantId AND ""IsCustomer"" = TRUE " + sqlsearch + " " + sqlsort + " LIMIT @jumlah_data OFFSET @offset";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", request.TenantId);
            parameters.Add("jumlah_data", request.jumlah_data);
            parameters.Add("offset", request.offset);

            using (var cn = _dbManager.CreateConnection())
            {
                List<Contact> result;
                result = (await cn.QueryAsync<Contact>(sql, parameters).ConfigureAwait(false)).ToList();
                return result;
            }
        }

        public async Task<Contact> UpdateCustomer(UpdateCustomer customer)
        {
            var sql = @"UPDATE ""Contact"" SET ""Name"" = @Name, ""Address"" = @Address, ""CityName"" = @CityName,""PostalCode"" = @PostalCode,""Email"" = @Email,""Phone"" = @Phone,""Fax"" = @Fax,""Website"" = @Website,""Npwp"" = @Npwp,""GroupId"" = @GroupId,""Notes"" = @Notes,""UpdatedUid"" = @UpdatedUid,""UpdatedUtc"" = @UpdatedUtc WHERE ""Id"" = @Id AND ""TenantId"" = @TenantId";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("Name", customer.Name);
            parameters.Add("Address", customer.Address);
            parameters.Add("CityName", customer.CityName);
            parameters.Add("PostalCode", customer.PostalCode);
            parameters.Add("Email", customer.Email);
            parameters.Add("Phone", customer.Phone);
            parameters.Add("Fax", customer.Fax);
            parameters.Add("Website", customer.Website);
            parameters.Add("Npwp", customer.Npwp);
            parameters.Add("GroupId", customer.GroupId);
            parameters.Add("Notes", customer.Notes);
            parameters.Add("UpdatedUid", customer.UpdatedUid);
            parameters.Add("UpdatedUtc", DateTime.UtcNow);
            parameters.Add("TenantId", customer.TenantId);
            parameters.Add("Id", customer.Id);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                Contact resultquery = new Contact();
                if (affected > 0)
                {
                    GetCustomerQuery param = new GetCustomerQuery();
                    param.TenantId = customer.TenantId;
                    param.Id = customer.Id;
                    resultquery = await GetCustomer(param);
                    return resultquery;
                }

                return resultquery;
            }
        }
    }
    public interface ICustomerRepository
    {
        Task<Contact> CreateCustomer(AddCustomer customer);
        Task<List<Contact>> GetCustomerList(GetCustomerListQuery request);
        Task<Contact> GetCustomer(GetCustomerQuery request);
        Task<Contact> UpdateCustomer(UpdateCustomer customer);
        Task<bool> DeleteCustomer(DeleteCustomer request);
    }
    public class VendorRepository : BaseRepository, IVendorRepository
    {
        public VendorRepository(DbManager dbContext) : base(dbContext)
        {
        }

        public async Task<Contact> CreateVendor(AddVendor customer)
        {
            var sql = @"INSERT INTO ""Contact"" (""TenantId"",""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid"",""CreatedUtc"", ""IsVendor"")
            VALUES(@TenantId, @Name, @Address, @CityName,@PostalCode,@Email,@Phone,@Fax,@Website,@Npwp,@GroupId,@Notes,@CreatedUid,@CreatedUtc,TRUE) RETURNING ""Id""";

            //var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", customer.TenantId);
            parameters.Add("Name", customer.Name);
            parameters.Add("Address", customer.Address);
            parameters.Add("CityName", customer.CityName);
            parameters.Add("PostalCode", customer.PostalCode);
            parameters.Add("Email", customer.Email);
            parameters.Add("Phone", customer.Phone);
            parameters.Add("Fax", customer.Fax);
            parameters.Add("Website", customer.Website);
            parameters.Add("Npwp", customer.Npwp);
            parameters.Add("GroupId", customer.GroupId);
            parameters.Add("Notes", customer.Notes);
            parameters.Add("CreatedUid", customer.CreatedUid);
            parameters.Add("CreatedUtc", DateTime.UtcNow);

            using (var cn = _dbManager.CreateConnection())
            {
                int affected = await cn.QueryFirstOrDefaultAsync<int>(sql, parameters);
                Contact resultquery = new Contact();
                GetVendorQuery param = new GetVendorQuery();
                param.TenantId = customer.TenantId;
                param.Id = affected;
                resultquery = await GetVendor(param);

                return resultquery;
            }
        }

        public async Task<bool> DeleteVendor(DeleteVendor request)
        {
            var sql = @"DELETE FROM ""Contact"" where IsVendor = true, ""Id"" = @id and ""TenantId"" = @TenantId";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("id", request.Id);
            parameters.Add("TenantId", request.TenantId);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }

        public async Task<Contact> GetVendor(GetVendorQuery request)
        {
            var sql = @"SELECT  ""Id"", ""TenantId"", ""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid""  FROM ""Contact"" WHERE ""TenantId"" = @TenantId AND ""IsVendor"" = TRUE AND ""Id"" = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", request.TenantId);
            parameters.Add("Id", request.Id);

            using (var cn = _dbManager.CreateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<Contact>(sql, parameters);
            }
        }

        public async Task<List<Contact>> GetVendorList(GetVendorListQuery request)
        {
            string sqlsort = "";
            string sqlsearch = "";
            if (request.sortby != null && request.valsort != null)
            {
                sqlsort = @" order by """ + request.sortby + "\" " + request.valsort + "";
            }
            if (request.searchby != null && request.valsearch != null)
            {
                sqlsearch = @"AND """ + request.searchby + "\" = '" + request.valsearch + "'";
            }

            var sql = @"SELECT ""Id"", ""TenantId"", ""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid""  FROM ""Contact"" WHERE ""TenantId"" = @TenantId AND ""IsVendor"" = TRUE " + sqlsearch + " " + sqlsort + " LIMIT @jumlah_data OFFSET @offset";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", request.TenantId);
            parameters.Add("jumlah_data", request.jumlah_data);
            parameters.Add("offset", request.offset);

            using (var cn = _dbManager.CreateConnection())
            {
                List<Contact> result;
                result = (await cn.QueryAsync<Contact>(sql, parameters).ConfigureAwait(false)).ToList();
                return result;
            }
        }

        public async Task<Contact> UpdateVendor(UpdateVendor customer)
        {
            var sql = @"UPDATE ""Contact"" SET ""Name"" = @Name, ""Address"" = @Address, ""CityName"" = @CityName,""PostalCode"" = @PostalCode,""Email"" = @Email,""Phone"" = @Phone,""Fax"" = @Fax,""Website"" = @Website,""Npwp"" = @Npwp,""GroupId"" = @GroupId,""Notes"" = @Notes,""UpdatedUid"" = @UpdatedUid,""UpdatedUtc"" = @UpdatedUtc WHERE ""Id"" = @Id AND ""TenantId"" = @TenantId";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("Name", customer.Name);
            parameters.Add("Address", customer.Address);
            parameters.Add("CityName", customer.CityName);
            parameters.Add("PostalCode", customer.PostalCode);
            parameters.Add("Email", customer.Email);
            parameters.Add("Phone", customer.Phone);
            parameters.Add("Fax", customer.Fax);
            parameters.Add("Website", customer.Website);
            parameters.Add("Npwp", customer.Npwp);
            parameters.Add("GroupId", customer.GroupId);
            parameters.Add("Notes", customer.Notes);
            parameters.Add("UpdatedUid", customer.UpdatedUid);
            parameters.Add("UpdatedUtc", DateTime.UtcNow);
            parameters.Add("TenantId", customer.TenantId);
            parameters.Add("Id", customer.Id);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                Contact resultquery = new Contact();
                if (affected > 0)
                {
                    GetVendorQuery param = new GetVendorQuery();
                    param.TenantId = customer.TenantId;
                    param.Id = customer.Id;
                    resultquery = await GetVendor(param);
                    return resultquery;
                }

                return resultquery;
            }
        }
    }
    public interface IVendorRepository
    {
        Task<Contact> CreateVendor(AddVendor customer);
        Task<List<Contact>> GetVendorList(GetVendorListQuery request);
        Task<Contact> GetVendor(GetVendorQuery request);
        Task<Contact> UpdateVendor(UpdateVendor customer);
        Task<bool> DeleteVendor(DeleteVendor request);
    }

    public class Contact
    {
        public int id { get; set; }
        public int Tenantid { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Npwp { get; set; }
        public int GroupId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsOther { get; set; }
        public string Notes { get; set; }
        public int CreatedUid { get; set; }
        public int UpdateUid { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdateUtc { get; set; }
    }
    public class contactpost {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Npwp { get; set; }
        public int GroupId { get; set; }
        public string Notes { get; set; }
    }

    public class getrequest
    {
        public string? sortby { get; set; }
        public string? valsort { get; set; }
        public string? searchby { get; set; }
        public string? valsearch { get; set; }
        public int? jumlah_data { get; set; }
        public int? offset { get; set; }
        public int? TenantId { get; set; }
    }

    public class customrespons 
    { 
        public string? status { get; set; }
        public string? message { get; set; }
    }
}
