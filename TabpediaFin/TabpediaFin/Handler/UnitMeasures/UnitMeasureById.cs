namespace TabpediaFin.Handler.UnitMeasure;

public class UnitMeasureById : IQueryByIdHandler<UnitMeasureDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public UnitMeasureById(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }
    public async Task<RowResponse<UnitMeasureDto>> Handle(QueryByIdDto<UnitMeasureDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<UnitMeasureDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<UnitMeasureDto>(request.Id, _currentUser);

                response.IsOk = true;
                response.Row = row;
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
