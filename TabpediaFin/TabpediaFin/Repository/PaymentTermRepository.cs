﻿using static TabpediaFin.Dto.UnitMeasureDto;

namespace TabpediaFin.Repository;

public class PaymentTermRepository : BaseRepository, IPaymentTermRepository
{
    public PaymentTermRepository(DbManager dbContext) : base(dbContext)
    { }
    public async Task<List<PaymentTerm>> GetPaymentTermList(GetPaymentTermListQuery request)
    {
        string sqlsearch = "";
        if (request.searchby != null && request.searchby != "")
        {
            sqlsearch = @"AND ""Name"" LIKE '%" + request.searchby + "%' " +
                " OR \"Description\" LIKE '%" + request.searchby + "%' ";
        }

        var sql = @"SELECT * FROM ""PaymentTerm"" WHERE ""TenantId"" = @TenantId " + sqlsearch + " ";

        var parameters = new DynamicParameters();
        parameters.Add("TenantId", request.TenantId);

        using (var cn = _dbManager.CreateConnection())
        {
            List<PaymentTerm> result;
            result = (await cn.QueryAsync<PaymentTerm>(sql, parameters).ConfigureAwait(false)).ToList();
            return result;
        }
    }
    public async Task<PaymentTerm> GetPaymentTermById(GetPaymentTermQuery request)
    {
        var sql = @"SELECT * FROM ""PaymentTerm"" WHERE ""TenantId"" = @TenantId AND ""Id"" = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("TenantId", request.TenantId);
        parameters.Add("Id", request.Id);

        using (var cn = _dbManager.CreateConnection())
        {
            return await cn.QueryFirstOrDefaultAsync<PaymentTerm>(sql, parameters);
        }
    }
    public async Task<PaymentTerm> CreatePaymentTerm(AddPaymentTerm paymentTerm)
    {
        var sql = @"INSERT INTO ""PaymentTerm"" 
                        (""TenantId"",""Name"", ""Description"",""TermDays"",""IsActive"",""CreatedUid"",""CreatedUtc"")
                        VALUES(@TenantId,@Name,@Description,@TermDays,@IsActive,@CreatedUid,@CreatedUtc) RETURNING ""Id""";

        //var expiredUtc = DateTime.UtcNow.AddDays(1);

        var parameters = new DynamicParameters();
        parameters.Add("TenantId", paymentTerm.TenantId);
        parameters.Add("Name", paymentTerm.Name);
        parameters.Add("Description", paymentTerm.Description);
        parameters.Add("TermDays", paymentTerm.TermDays);
        parameters.Add("IsActive", paymentTerm.IsActive);
        parameters.Add("CreatedUid", paymentTerm.CreatedUid);
        parameters.Add("CreatedUtc", DateTime.UtcNow);

        using (var cn = _dbManager.CreateConnection())
        {
            int affected = await cn.QueryFirstOrDefaultAsync<int>(sql, parameters);
            PaymentTerm resultQuery = new PaymentTerm();
            GetPaymentTermQuery param = new GetPaymentTermQuery();
            param.TenantId = paymentTerm.TenantId;
            param.Id = affected;
            resultQuery = await GetPaymentTermById(param);

            return resultQuery;
        }
    }
    public async Task<PaymentTerm> UpdatePaymentTerm(UpdatePaymentTerm paymentTerm)
    {
        var sql = @"UPDATE ""PaymentTerm"" 
                        SET ""Name"" = @Name, ""Description"" = @Description, ""TermDays"" = @TermDays, ""IsActive"" = @IsActive,""UpdatedUid"" = @UpdatedUid,""UpdatedUtc"" = @UpdatedUtc 
                        WHERE ""Id"" = @Id AND ""TenantId"" = @TenantId";

        var expiredUtc = DateTime.UtcNow.AddDays(1);

        var parameters = new DynamicParameters();
        parameters.Add("Name", paymentTerm.Name);
        parameters.Add("Description", paymentTerm.Description);
        parameters.Add("TermDays", paymentTerm.TermDays);
        parameters.Add("IsActive", paymentTerm.IsActive);
        parameters.Add("UpdatedUid", paymentTerm.UpdatedUid);
        parameters.Add("UpdatedUtc", DateTime.UtcNow);
        parameters.Add("TenantId", paymentTerm.TenantId);
        parameters.Add("Id", paymentTerm.Id);

        using (var cn = _dbManager.CreateConnection())
        {
            var affected = await cn.ExecuteAsync(sql, parameters);
            PaymentTerm resultQuery = new PaymentTerm();
            if (affected > 0)
            {
                GetPaymentTermQuery param = new GetPaymentTermQuery();
                param.TenantId = paymentTerm.TenantId;
                param.Id = paymentTerm.Id;
                resultQuery = await GetPaymentTermById(param);
                return resultQuery;
            }

            return resultQuery;
        }
    }
    public async Task<bool> DeletePaymentTerm(DeletePaymentTerm request)
    {
        var sql = @"DELETE FROM ""PaymentTerm"" where ""Id"" = @Id and ""TenantId"" = @TenantId";

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

}


public class PaymentTerm
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TermDays { get; set; }
    public bool IsActive { get; set; }
    public int CreatedUid { get; set; }
    public DateTime CreatedUtc { get; set; }
    public int UpdatedUid { get; set; }
    public DateTime UpdatedUtc { get; set; }
}

public class PaymentTermRespons
{
    public string? status { get; set; }
    public string? message { get; set; }
}


public interface IPaymentTermRepository
{
    Task<PaymentTerm> CreatePaymentTerm(AddPaymentTerm customer);
    Task<List<PaymentTerm>> GetPaymentTermList(GetPaymentTermListQuery request);
    Task<PaymentTerm> GetPaymentTermById(GetPaymentTermQuery request);
    Task<PaymentTerm> UpdatePaymentTerm(UpdatePaymentTerm customer);
    Task<bool> DeletePaymentTerm(DeletePaymentTerm request);
}