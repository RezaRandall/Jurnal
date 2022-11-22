using TabpediaFin.Handler.Item;

namespace TabpediaFin.Handler.UnitMeasures;

public class UnitMeasureInsertHandler : IRequestHandler<UnitMeasureInsertDto, RowResponse<UnitMeasureDto>>
{
    private readonly FinContext _context;
    public UnitMeasureInsertHandler(FinContext db)
    {
        _context = db;
    }

    public class CommandValidator : AbstractValidator<UnitMeasureInsertDto>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).MaximumLength(100);
        }
    }

    public async Task<RowResponse<UnitMeasureDto>> Handle(UnitMeasureInsertDto req, CancellationToken cancellationToken)
    {
        var result = new RowResponse<UnitMeasureDto>();

        var unitMeasure = new Domain.UnitMeasure()
        {
            Name = req.Name
            ,Description = req.Description
        };

        try
        {
            await _context.UnitMeasure.AddAsync(unitMeasure, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new UnitMeasureDto()
            {
                Id = unitMeasure.Id
                ,Name = unitMeasure.Name
                ,Description = unitMeasure.Description
            };

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
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
public class UnitMeasureInsertDto : IRequest<RowResponse<UnitMeasureDto>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
