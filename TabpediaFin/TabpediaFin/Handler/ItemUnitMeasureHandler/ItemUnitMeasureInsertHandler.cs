using TabpediaFin.Handler.ContactAddressHandler;


namespace TabpediaFin.Handler.ItemUnitMeasureHandler;

//public class ItemUnitMeasureInsertHandler : IRequestHandler<ItemUnitMeasureInsertDto, RowResponse<ItemUnitMeasureFetchDto>>
//{

//}

//[Table("ItemUnitMeasure")]
public class ItemUnitMeasureInsertDto : IRequest<RowResponse<ItemUnitMeasureFetchDto>>
{
    public int Id { get; set; } = 0;
    public int UnitMeasureId { get; set; } = 0;
    public int UnitConversion { get; set; } = 0;
    public int Cost { get; set; } = 0;
    public int Price { get; set; } = 0;
    public string Notes { get; set; } = string.Empty;
}