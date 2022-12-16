namespace TabpediaFin.Handler.BankNameHandler;

public class BankNameFetchHandler : IFetchByIdHandler<BankNameFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;
    public BankNameFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<BankNameFetchDto>> Handle(FetchByIdRequestDto<BankNameFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<BankNameFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<BankNameFetchDto>(request.Id, _currentUser);

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

[Table("BankName")]
public class BankNameFetchDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
}
