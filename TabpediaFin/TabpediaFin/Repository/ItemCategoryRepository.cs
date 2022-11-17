namespace TabpediaFin.Repository
{
    public class ItemCategoryRepository : BaseRepository, IItemCategoryRepository
    {
        public ItemCategoryRepository(DbManager dbManager) : base(dbManager)
        {
        }

        public async Task<ItemCategory> CreateItemCategory(AddItemCategory item)
        {
            var sql = @"INSERT INTO ""ItemCategory"" (""TenantId"",""Name"", ""Description"",""CreatedUid"",""CreatedUtc"")
            VALUES(@TenantId, @Name, @Description, @CreatedUid, @CreatedUtc) RETURNING ""Id""";

            //var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", item.TenantId);
            parameters.Add("Description", item.Description);
            parameters.Add("CreatedUid", item.CreatedUid);
            parameters.Add("CreatedUtc", DateTime.UtcNow);

            using (var cn = _dbManager.CreateConnection())
            {
                int affected = await cn.QueryFirstOrDefaultAsync<int>(sql, parameters);
                ItemCategory resultquery = new ItemCategory();
                GetItemCategoryQuery param = new GetItemCategoryQuery();
                param.TenantId = item.TenantId;
                param.Id = affected;
                resultquery = await GetItemCategory(param);

                return resultquery;
            }
        }

        public async Task<bool> DeleteItemCategory(DeleteItemCategory item)
        {
            var sql = @"DELETE FROM ""ItemCategory"" where ""Id"" = @Id and ""TenantId"" = @TenantId";

            var parameters = new DynamicParameters();
            parameters.Add("Id", item.Id);
            parameters.Add("TenantId", item.TenantId);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                return affected > 0;
            }
        }

        public async Task<ItemCategory> GetItemCategory(GetItemCategoryQuery item)
        {
            var sql = @"SELECT u.""FullName"" as editor, a.""FullName"" as creator, i.""Id"", i.""TenantId"",i.""Name"", i.""Description"", i.""CreatedUid"",i.""CreatedUtc"",i.""UpdatedUid"", i.""UpdatedUtc""  FROM ""ItemCategory"" i LEFT JOIN ""AppUser"" a ON i.""CreatedUid"" = a.""Id"" LEFT JOIN ""AppUser"" u ON i.""UpdatedUid"" = a.""Id""  WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", item.TenantId);
            parameters.Add("Id", item.Id);

            using (var cn = _dbManager.CreateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<ItemCategory>(sql, parameters);
            }
        }

        public async Task<List<ItemCategory>> GetItemCategoryList(GetItemCategoryListQuery item)
        {
            string sqlsort = "";
            string sqlsearch = "";
            if (item.sortby != null && item.valsort != null && item.sortby != "" && item.valsort != "")
            {
                sqlsort = @" order by i.""" + item.sortby + "\" " + item.valsort + "";
            }
            if (item.searchby != null && item.valsearch != null && item.searchby != "" && item.valsearch != "")
            {
                sqlsearch = @"AND i.""" + item.searchby + "\" = '" + item.valsearch + "'";
            }

            var sql = @"SELECT u.""FullName"" as editor, a.""FullName"" as creator, i.""Id"", i.""TenantId"",i.""Name"", i.""Description"", i.""CreatedUid"",i.""CreatedUtc"",i.""UpdatedUid"", i.""UpdatedUtc""  FROM ""ItemCategory"" i LEFT JOIN ""AppUser"" a ON i.""CreatedUid"" = a.""Id"" LEFT JOIN ""AppUser"" u ON i.""UpdatedUid"" = a.""Id"" WHERE i.""TenantId"" = @TenantId " + sqlsearch + " " + sqlsort + " LIMIT @jumlah_data OFFSET @offset";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", item.TenantId);
            parameters.Add("jumlah_data", item.jumlah_data);
            parameters.Add("offset", item.offset);

            using (var cn = _dbManager.CreateConnection())
            {
                List<ItemCategory> result;
                result = (await cn.QueryAsync<ItemCategory>(sql, parameters).ConfigureAwait(false)).ToList();
                return result;
            }
        }

        public async Task<ItemCategory> UpdateItemCategory(UpdateItemCategory item)
        {
            var sql = @"UPDATE ""ItemCategory"" SET ""Name"" = @Name, ""Description"" = @Description, ""UpdatedUid"" = @UpdatedUid,""UpdatedUtc"" = @UpdatedUtc WHERE ""Id"" = @Id AND ""TenantId"" = @TenantId";

            var expiredUtc = DateTime.UtcNow.AddDays(1);

            var parameters = new DynamicParameters();
            parameters.Add("Name", item.Name);
            parameters.Add("Description", item.Description);
            parameters.Add("UpdatedUid", item.UpdatedUid);
            parameters.Add("UpdatedUtc", DateTime.UtcNow);
            parameters.Add("TenantId", item.TenantId);
            parameters.Add("Id", item.Id);

            using (var cn = _dbManager.CreateConnection())
            {
                var affected = await cn.ExecuteAsync(sql, parameters);
                ItemCategory resultquery = new ItemCategory();
                if (affected > 0)
                {
                    GetItemCategoryQuery param = new GetItemCategoryQuery();
                    param.TenantId = item.TenantId;
                    param.Id = item.Id;
                    resultquery = await GetItemCategory(param);
                    return resultquery;
                }

                return resultquery;
            }
        }
    }
    public interface IItemCategoryRepository
    {
        Task<ItemCategory> CreateItemCategory(AddItemCategory item);
        Task<List<ItemCategory>> GetItemCategoryList(GetItemCategoryListQuery item);
        Task<ItemCategory> GetItemCategory(GetItemCategoryQuery item);
        Task<ItemCategory> UpdateItemCategory(UpdateItemCategory item);
        Task<bool> DeleteItemCategory(DeleteItemCategory item);
    }
    public class ItemCategory
    {
        public int id { get; set; }
        public int Tenantid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string creator { get; set; }
        public string editor { get; set; }
        public int CreatedUid { get; set; }
        public int UpdateUid { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
    }
    public class ItemCategoryPost
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
