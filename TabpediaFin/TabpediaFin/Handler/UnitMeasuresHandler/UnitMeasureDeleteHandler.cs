using TabpediaFin.Handler.Item;
using TabpediaFin.Handler.PaymentTerm;

namespace TabpediaFin.Handler.UnitMeasures;

public class UnitMeasureDeleteHandler : IRequestHandler<UnitMeasureDeleteDto, RowResponse<bool>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public UnitMeasureDeleteHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public class CommandValidator : AbstractValidator<UnitMeasureDeleteDto>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }

    public async Task<RowResponse<bool>> Handle(UnitMeasureDeleteDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<bool>();
        try
        {
            var unitMeasureData = await _context.UnitMeasure.FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            if (unitMeasureData != null)
            {
                _context.UnitMeasure.Remove(unitMeasureData);
                result.IsOk = true;
                result.ErrorMessage = "Unit Measure with id " + request.Id + " has been deleted";
            }
            if (unitMeasureData == null)
            {
                result.IsOk = false;
                result.ErrorMessage = "Data not found";
            }
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

[Table("UnitMeasure")]
public class UnitMeasureDeleteDto : IRequest<RowResponse<bool>>
{
    public int Id { get; set; } = 0;
    public int TenantId { get; set; } = 0;
}
