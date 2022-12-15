namespace TabpediaFin.Handler.UnitMeasures;

public class UnitMeasureUpdateHandler : IRequestHandler<UnitMeasureUpdateDto, RowResponse<UnitMeasureDto>>
{
    private readonly FinContext _context;

    public UnitMeasureUpdateHandler(FinContext db)
    {
        _context = db;
    }

    public class CommandValidator : AbstractValidator<UnitMeasureUpdateDto>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).MaximumLength(100); ;
        }
    }

    public async Task<RowResponse<UnitMeasureDto>> Handle(UnitMeasureUpdateDto req, CancellationToken cancellationToken)
    {
        var result = new RowResponse<UnitMeasureDto>();

        try
        {
            var unitMeasure = await _context.UnitMeasure.FirstAsync(x => x.Id == req.Id, cancellationToken);
            unitMeasure.Name = req.Name;
            unitMeasure.Description = req.Description;

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
public class UnitMeasureUpdateDto : IRequest<RowResponse<UnitMeasureDto>>
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
