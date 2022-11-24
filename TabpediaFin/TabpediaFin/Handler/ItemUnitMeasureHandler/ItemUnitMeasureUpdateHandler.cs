namespace TabpediaFin.Handler.ItemUnitMeasureHandler;

public class ItemUnitMeasureUpdateHandler
{
}

public class ItemUnitMeasureUpdateDto : IRequest<RowResponse<ItemUnitMeasureFetchDto>>
{
    public int UnitMeasureId { get; set; } = 0;
    public int ItemId { get; set; } = 0;
    public int UnitConversion { get; set; } = 0;
    public int Cost { get; set; } = 0;
    public int Price { get; set; } = 0;
    public string Notes { get; set; } = string.Empty;
}