namespace TabpediaFin.Handler.UnitMeasures;

public class UnitMeasureByIdHandler : IQueryByIdHandler<UnitMeasureDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public UnitMeasureByIdHandler(DbManager dbManager, ICurrentUser currentUser)
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
                if (row == null)
                {
                    response.IsOk = false;
                    response.Row = row;
                    response.ErrorMessage = "Data not found";
                }
                else
                {
                    response.IsOk = true;
                    response.Row = row;
                    response.ErrorMessage = string.Empty;
                }
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

[Table("UnitMeasure")]
public class UnitMeasureDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
