namespace TabpediaFin.Handler.StockHandler;

public class StockOpnameUpdateHandler : IRequestHandler<StockOpnameUpdateList, PagedListResponse<StockProductListDto>>
{
    private readonly FinContext _context;
    //private readonly IContactGroupCacheRemover _cacheRemover;

    public StockOpnameUpdateHandler(FinContext db/*, IContactGroupCacheRemover cacheRemover*/)
    {
        _context = db;
        //_cacheRemover = cacheRemover;
    }

    public async Task<PagedListResponse<StockProductListDto>> Handle(StockOpnameUpdateList request, CancellationToken cancellationToken)
    {
        var result = new PagedListResponse<StockProductListDto>();

        var ItemStock = new List<ItemStock>();

        try
        {
            //await _context.ItemStock.AddAsync(ItemStock, cancellationToken);
            //await _context.SaveChangesAsync(cancellationToken);

            //_cacheRemover.RemoveCache();

            var row = new List<StockProductListDto>();
            foreach (StockOpnameUpdate Item in request.StockOpnameList)
            {
                ItemStock.Add(new ItemStock
                {
                    Id = Item.Id,
                    WarehouseId = Item.WarehouseId,
                    ItemId = Item.ItemId,
                    Quantity = Item.Quantity,
                    Cost = Item.Cost,
                    Price = Item.Price,
                });
                row.Add(new StockProductListDto
                {
                    WarehouseId = Item.WarehouseId,
                    ItemId = Item.ItemId,
                    Stock = Item.Quantity,
                    Cost = Item.Cost,
                    Price = Item.Price,
                });
            }

            _context.ItemStock.UpdateRange(ItemStock);
            await _context.SaveChangesAsync(cancellationToken);

            result.RecordCount = row.Count;

            result.IsOk = true;
            result.List = row;
            result.ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
}

public class StockOpnameUpdateList : IRequest<PagedListResponse<StockProductListDto>>
{
    public List<StockOpnameUpdate> StockOpnameList { get; set; }
}

public class StockOpnameUpdate
{
    public int Id { get; set; }
    public int WarehouseId { get; set; } = 0;
    public int ItemId { get; set; } = 0;
    public double Quantity { get; set; } = 0;
    public double Cost { get; set; } = 0;
    public double Price { get; set; } = 0;
}
//public class StockOpnameUpdateValidator : AbstractValidator<StockOpnameUpdateList>
//{
//    private readonly IUniqueNameValidationRepository _repository;

//    public StockOpnameUpdateValidator(IUniqueNameValidationRepository repository)
//    {
//        _repository = repository;

//        RuleFor(x => x.WarehouseId)
//            .NotNull()
//            .NotEmpty();

//        RuleFor(x => x.ItemId)
//            .NotNull()
//            .NotEmpty();
//        RuleFor(x => x.Quantity)
//            .NotNull()
//            .NotEmpty();
//    }
//}
