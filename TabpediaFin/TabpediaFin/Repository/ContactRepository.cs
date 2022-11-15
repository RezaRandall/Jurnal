namespace TabpediaFin.Repository
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(DbManager dbContext) : base(dbContext)
        {

        }

        public async Task<bool> CreateCustomer(contactpost customer, int CreatedUid, int TenantId)
        {
            var sql = @"INSERT INTO ""Contact"" (""TenantId"",""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid"",""CreatedUtc"", ""IsCustomer"")
            VALUES(@Name, @Address, @CityName,@PostalCode,@Email,@Phone,@Fax,@Website,@Npwp,@GroupId,@Notes,@CreatedUid,@CreatedUtc,TRUE)";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", TenantId);
            parameters.Add("Name", customer.Name);
            parameters.Add("Address",customer.Address);
            parameters.Add("CityName",customer.CityName);
            parameters.Add("PostalCode",customer.PostalCode);
            parameters.Add("Email",customer.Email);
            parameters.Add("Phone",customer.Phone);
            parameters.Add("Fax",customer.Fax);
            parameters.Add("Website",customer.Website);
            parameters.Add("Npwp",customer.Npwp);
            parameters.Add("GroupId",customer.GroupId);
            parameters.Add("Notes",customer.Notes);
            parameters.Add("CreatedUid", CreatedUid);
            parameters.Add("CreatedUtc", DateTime.UtcNow);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }

        public async Task<bool> DeleteCustomer(int id, int TenantId)
        {
            var sql = @"DELETE FROM ""Contact"" where ""Id"" = @id and ""TenantId"" = @TenantId";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            parameters.Add("TenantId", TenantId);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }

        public async Task<Contact> GetCustomer(int TenantId, int id)
        {
            var sql = @"SELECT ""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid""  FROM ""Contact"" WHERE ""TenantId"" = @TenantId AND ""IsCustomer"" = TRUE ""id"" = @id";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", TenantId);
            parameters.Add("id", id);

            using (var cn = _dbManager.CreateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<Contact>(sql, parameters);
            }
        }

        public async Task<List<Contact>> GetCustomerList(string? sortby, string? valsort, string? searchby, string? valsearch, int? jumlah_data, int? offset, int? TenantId)
        {
            string sqlsort = "";
            string sqlsearch = "";
            if (sortby != null && valsort != null)
            {
                sqlsort = @" order by """ + sortby + "\" " + valsort + "";
            }
            if (searchby != null && valsearch != null)
            {
                sqlsearch = @"AND """ + searchby + "\" = '" + valsearch + "'";
            }

            var sql = @"SELECT ""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid""  FROM ""Contact"" WHERE ""TenantId"" = @TenantId AND ""IsCustomer"" = TRUE " + sqlsearch + " " + sqlsort + " LIMIT @jumlah_data OFFSET @offset";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", TenantId);
            parameters.Add("jumlah_data", jumlah_data);
            parameters.Add("offset", offset);

            using (var cn = _dbManager.CreateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<List<Contact>>(sql, parameters);
            }
        }

        public async Task<bool> UpdateCustomer(contactpost customer, int UpdatedUid, int TenantId, int id)
        {
            var sql = @"UPDATE ""Contact"" SET ""TenantId"" = @TenantId,""Name"" = @Name, ""Address"" = @Address, ""CityName"" = @CityName,""PostalCode"" = @PostalCode,""Email"" = @Email,""Phone"" = @Phone,""Fax"" = @Fax,""Website"" = @Website,""Npwp"" = @Npwp,""GroupId"" = @GroupId,""Notes"" = @Notes,""UpdatedUid"" = @UpdatedUid,""UpdatedUtc"" = @UpdatedUtc WHERE ""id"" = @id";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", TenantId);
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
            parameters.Add("UpdatedUid", UpdatedUid);
            parameters.Add("UpdatedUtc", DateTime.UtcNow);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }
    }
    public interface ICustomerRepository
    {
        Task<bool> CreateCustomer(contactpost customer, int TenantId, int CreatedUid);
        Task<List<Contact>> GetCustomerList(string? sortby, string? valsort, string? searchby, string? valsearch, int? jumlah_data, int? offset, int? TenantId);
        Task<Contact> GetCustomer(int TenantId, int id);
        Task<bool> UpdateCustomer(contactpost customer, int UpdatedUid, int TenantId, int id);
        Task<bool> DeleteCustomer(int id, int TenantId);
    }
    public class VendorRepository : BaseRepository, IVendorRepository
    {
        public VendorRepository(DbManager dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CreateVendor(contactpost customer, int TenantId, int CreatedUid)
        {
            var sql = @"INSERT INTO ""Contact"" (""TenantId"",""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid"",""CreatedUtc"", ""IsVendor"")
            VALUES(@Name, @Address, @CityName,@PostalCode,@Email,@Phone,@Fax,@Website,@Npwp,@GroupId,@Notes,@CreatedUid,@CreatedUtc,TRUE)";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", TenantId);
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
            parameters.Add("CreatedUid", CreatedUid);
            parameters.Add("CreatedUtc", DateTime.UtcNow);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }

        public async Task<bool> DeleteVendor(int id, int TenantId)
        {
            var sql = @"DELETE FROM ""Contact"" where ""Id"" = @id and ""TenantId"" = @TenantId";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            parameters.Add("TenantId", TenantId);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }

        public async Task<Contact> GetVendor(int TenantId, int id)
        {
            var sql = @"SELECT ""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid""  FROM ""Contact"" WHERE ""TenantId"" = @TenantId AND ""IsVendor"" = TRUE ""id"" = @id";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", TenantId);
            parameters.Add("id", id);

            using (var cn = _dbManager.CreateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<Contact>(sql, parameters);
            }
        }

        public async Task<List<Contact>> GetVendorList(string? sortby, string? valsort, string? searchby, string? valsearch, int? jumlah_data, int? offset, int? TenantId)
        {
            string sqlsort = "";
            string sqlsearch = "";
            if (sortby != null && valsort != null)
            {
                sqlsort = @" order by """ + sortby + "\" " + valsort + "";
            }
            if (searchby != null && valsearch != null)
            {
                sqlsearch = @"AND """ + searchby + "\" = '" + valsearch + "'";
            }

            var sql = @"SELECT ""Name"", ""Address"", ""CityName"",""PostalCode"",""Email"",""Phone"",""Fax"",""Website"",""Npwp"",""GroupId"",""Notes"",""CreatedUid""  FROM ""Contact"" WHERE ""TenantId"" = @TenantId AND ""IsVendor"" = TRUE " + sqlsearch + " " + sqlsort + " LIMIT @jumlah_data OFFSET @offset";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", TenantId);
            parameters.Add("jumlah_data", jumlah_data);
            parameters.Add("offset", offset);

            using (var cn = _dbManager.CreateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<List<Contact>>(sql, parameters);
            }
        }

        public async Task<bool> UpdateVendor(contactpost customer, int UpdatedUid, int TenantId, int id)
        {
            var sql = @"UPDATE ""Contact"" SET ""TenantId"" = @TenantId,""Name"" = @Name, ""Address"" = @Address, ""CityName"" = @CityName,""PostalCode"" = @PostalCode,""Email"" = @Email,""Phone"" = @Phone,""Fax"" = @Fax,""Website"" = @Website,""Npwp"" = @Npwp,""GroupId"" = @GroupId,""Notes"" = @Notes,""UpdatedUid"" = @UpdatedUid,""UpdatedUtc"" = @UpdatedUtc WHERE ""id"" = @id";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", TenantId);
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
            parameters.Add("UpdatedUid", UpdatedUid);
            parameters.Add("UpdatedUtc", DateTime.UtcNow);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }
    }
    public interface IVendorRepository
    {
        Task<bool> CreateVendor(contactpost customer, int TenantId, int CreatedUid);
        Task<List<Contact>> GetVendorList(string? sortby, string? valsort, string? searchby, string? valsearch, int? jumlah_data, int? offset, int? TenantId);
        Task<Contact> GetVendor(int TenantId, int id);
        Task<bool> UpdateVendor(contactpost customer, int UpdatedUid, int TenantId, int id);
        Task<bool> DeleteVendor(int id, int TenantId);
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
}
