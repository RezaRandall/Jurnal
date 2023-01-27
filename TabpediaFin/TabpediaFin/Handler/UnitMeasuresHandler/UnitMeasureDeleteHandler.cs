namespace TabpediaFin.Handler.UnitMeasures;

public class UnitMeasureDeleteHandler : IDeleteByIdHandler<UnitMeasureDto>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public UnitMeasureDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }


    public async Task<RowResponse<UnitMeasureDto>> Handle(DeleteByIdRequestDto<UnitMeasureDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<UnitMeasureDto>();
        try
        {
            var unitMeasure = await _context.UnitMeasure.FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (unitMeasure == null || unitMeasure.Id == 0)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Data not found");
            }
            _context.UnitMeasure.Remove(unitMeasure);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


}

