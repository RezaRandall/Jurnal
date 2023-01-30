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
            ,
            Description = req.Description
        };

        try
        {
            await _context.UnitMeasure.AddAsync(unitMeasure, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new UnitMeasureDto()
            {
                Id = unitMeasure.Id,
                Name = unitMeasure.Name,
                Description = unitMeasure.Description
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



// INSERT INTO MULTIPLE TABLE 

//public class UnitMeasureInsertHandler : IRequestHandler<UnitMeasureInsertDto, RowResponse<UnitMeasureDto>>
//{
//    private readonly FinContext _context;
//    public UnitMeasureInsertHandler(FinContext db)
//    {
//        _context = db;
//    }

//    public class CommandValidator : AbstractValidator<UnitMeasureInsertDto>
//    {
//        public CommandValidator()
//        {
//            RuleFor(x => x.Name).NotNull().NotEmpty();
//            RuleFor(x => x.Description).MaximumLength(100);
//        }
//    }

//    public async Task<RowResponse<UnitMeasureDto>> Handle(UnitMeasureInsertDto request, CancellationToken cancellationToken)
//    {
//        var result = new RowResponse<UnitMeasureDto>();
//        int unitMeasureIdResult;
//        var unitMeasure = new UnitMeasure()
//        {
//            Name = request.Name,
//            Description = request.Description,
//        };
//        List<ItemUnitMeasure> ItemUnitMeasure = new List<ItemUnitMeasure>();
//        List<ItemUnitMeasureFetchDto> ItemUnitMeasureFetchDto = new List<ItemUnitMeasureFetchDto>();

//        try
//        {
//            await _context.UnitMeasure.AddAsync(unitMeasure, cancellationToken);
//            await _context.SaveChangesAsync(cancellationToken);
//            unitMeasureIdResult = unitMeasure.Id;
//            if (request.ItemUnitMeasureList.Count > 0)
//            {
//                foreach (ItemUnitMeasureInsertDto item in request.ItemUnitMeasureList)
//                {
//                    ItemUnitMeasure.Add(new ItemUnitMeasure
//                    {
//                        UnitMeasureId = unitMeasureIdResult,
//                        UnitConversion = item.UnitConversion,
//                        Cost = item.Cost,
//                        Price = item.Price,
//                        Notes = item.Notes
//                    });
//                    ItemUnitMeasureFetchDto.Add(new ItemUnitMeasureFetchDto
//                    {
//                        UnitMeasureId = unitMeasureIdResult,
//                        UnitConversion = item.UnitConversion,
//                        Cost = item.Cost,
//                        Price = item.Price,
//                        Notes = item.Notes
//                    });
//                }
//                await _context.ItemUnitMeasure.AddRangeAsync(ItemUnitMeasure, cancellationToken);
//            }
//            if (ItemUnitMeasure.Count > 0)
//            {
//                await _context.SaveChangesAsync(cancellationToken);
//            }
//            var row = new UnitMeasureDto()
//            {
//                Name = unitMeasure.Name,
//                Description = unitMeasure.Description
//            };
//            result.IsOk = true;
//            result.Row = row;
//            result.ErrorMessage = string.Empty;
//        }
//        catch (Exception ex)
//        {
//            result.IsOk = false;
//            result.ErrorMessage = ex.Message;
//        }
//        return result;
//    }
//}

//[Table("UnitMeasure")]
//public class UnitMeasureInsertDto : IRequest<RowResponse<UnitMeasureDto>>
//{
//    public string Name { get; set; } = string.Empty;
//    public string Description { get; set; } = string.Empty;
//    public List<ItemUnitMeasureInsertDto> ItemUnitMeasureList { get; set; }
//}