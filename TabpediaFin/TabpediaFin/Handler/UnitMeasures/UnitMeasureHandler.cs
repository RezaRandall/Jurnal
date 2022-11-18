using TabpediaFin.Infrastructure.Security;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TabpediaFin.Handler.UnitMeasure;

public class UnitMeasureHandler : BaseRepository, IUnitMeasure
{
    public UnitMeasureHandler(DbManager dbContext) : base(dbContext)
    { }
    public async Task<UnitMeasure> GetUnitMeasureById(GetUnitMeasureQuery request)
    {
        var sql = @"SELECT *  FROM ""UnitMeasure"" WHERE ""TenantId"" = @TenantId AND ""Id"" = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("TenantId", request.TenantId);
        parameters.Add("Id", request.Id);

        using (var cn = _dbManager.CreateConnection())
        {
            return await cn.QueryFirstOrDefaultAsync<UnitMeasure>(sql, parameters);
        }
    }

    public async Task<UnitMeasure> CreateUnitMeasures(AddUnitMeasure unitMeasure)
    {
        var sql = @"INSERT INTO ""UnitMeasure"" 
                        (""TenantId"",""Name"", ""Description"",""CreatedUid"",""CreatedUtc"")
                        VALUES(@TenantId,@Name,@Description,@CreatedUid,@CreatedUtc) RETURNING ""Id""";

        //var expiredUtc = DateTime.UtcNow.AddDays(1);

        var parameters = new DynamicParameters();
        parameters.Add("TenantId", unitMeasure.TenantId);
        parameters.Add("Name", unitMeasure.Name);
        parameters.Add("Description", unitMeasure.Description);
        parameters.Add("CreatedUid", unitMeasure.CreatedUid);
        parameters.Add("CreatedUtc", DateTime.UtcNow);

        using (var cn = _dbManager.CreateConnection())
        {
            int affected = await cn.QueryFirstOrDefaultAsync<int>(sql, parameters);
            UnitMeasure resultQuery = new UnitMeasure();
            GetUnitMeasureQuery param = new GetUnitMeasureQuery();
            param.TenantId = unitMeasure.TenantId;
            param.Id = affected;
            resultQuery = await GetUnitMeasureById(param);

            return resultQuery;
        }
    }

    public async Task<UnitMeasure> UpdateUnitMeasure(UpdateUnitMeasure unitMeasure)
    {
        var sql = @"UPDATE ""UnitMeasure"" 
                            SET ""Name"" = @Name, ""Description"" = @Description,""UpdatedUid"" = @UpdatedUid,""UpdatedUtc"" = @UpdatedUtc 
                            WHERE ""Id"" = @Id AND ""TenantId"" = @TenantId";

        var expiredUtc = DateTime.UtcNow.AddDays(1);

        var parameters = new DynamicParameters();
        parameters.Add("Name", unitMeasure.Name);
        parameters.Add("Description", unitMeasure.Description);
        parameters.Add("UpdatedUid", unitMeasure.UpdatedUid);
        parameters.Add("UpdatedUtc", DateTime.UtcNow);
        parameters.Add("TenantId", unitMeasure.TenantId);
        parameters.Add("Id", unitMeasure.Id);

        using (var cn = _dbManager.CreateConnection())
        {
            var affected = await cn.ExecuteAsync(sql, parameters);
            UnitMeasure resultQuery = new UnitMeasure();
            if (affected > 0)
            {
                GetUnitMeasureQuery param = new GetUnitMeasureQuery();
                param.TenantId = unitMeasure.TenantId;
                param.Id = unitMeasure.Id;
                resultQuery = await GetUnitMeasureById(param);
                return resultQuery;
            }

            return resultQuery;
        }
    }

    public async Task<bool> DeleteUnitMeasure(DeleteUnitMeasure request)
    {
        var sql = @"DELETE FROM ""UnitMeasure"" where ""Id"" = @Id and ""TenantId"" = @TenantId";

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




    public class AddUnitMeasureHandler : IRequestHandler<AddUnitMeasure, UnitMeasure>
    {
        private readonly IUnitMeasure _unitMeasure;
        public AddUnitMeasureHandler(IUnitMeasure unitMeasure)
        {
            _unitMeasure = unitMeasure;
        }

        async Task<UnitMeasure> IRequestHandler<AddUnitMeasure, UnitMeasure>.Handle(AddUnitMeasure request, CancellationToken cancellationToken)
        {
            var result = await _unitMeasure.CreateUnitMeasures(request);
            return result;
        }
    }

    public class UpdateUnitMeasureHandler : IRequestHandler<UpdateUnitMeasure, UnitMeasure>
    {
        private readonly IUnitMeasure _unitMeasure;
        public UpdateUnitMeasureHandler(IUnitMeasure unitMeasure)
        {
            _unitMeasure = unitMeasure;
        }

        async Task<UnitMeasure> IRequestHandler<UpdateUnitMeasure, UnitMeasure>.Handle(UpdateUnitMeasure request, CancellationToken cancellationToken)
        {
            var result = await _unitMeasure.UpdateUnitMeasure(request);
            return result;
        }
    }

    public class DeleteUnitMeasureHandler : IRequestHandler<DeleteUnitMeasure, bool>
    {
        private readonly IUnitMeasure _unitMeasure;
        public DeleteUnitMeasureHandler(IUnitMeasure unitMeasure)
        {
            _unitMeasure = unitMeasure;
        }

        public async Task<bool> Handle(DeleteUnitMeasure request, CancellationToken cancellationToken)
        {
            var result = await _unitMeasure.DeleteUnitMeasure(request);
            return result;
        }
    }
}
public class UnitMeasure
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatedUid { get; set; }
    public DateTime CreatedUtc { get; set; }
    public int UpdatedUid { get; set; }
    public DateTime UpdatedUtc { get; set; }
}

public class UnitMeasureRespons
{
    public string? status { get; set; }
    public string? message { get; set; }
}


public interface IUnitMeasure
{
    Task<UnitMeasure> CreateUnitMeasures(AddUnitMeasure customer);
    Task<UnitMeasure> GetUnitMeasureById(GetUnitMeasureQuery request);
    Task<UnitMeasure> UpdateUnitMeasure(UpdateUnitMeasure customer);
    Task<bool> DeleteUnitMeasure(DeleteUnitMeasure request);
}

