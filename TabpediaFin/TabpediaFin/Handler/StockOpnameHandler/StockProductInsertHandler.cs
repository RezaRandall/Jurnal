namespace TabpediaFin.Handler.StockHandler;

public class StockOpnameInsertHandler : IRequestHandler<StockOpnameInsertList, PagedListResponse<StockProductListDto>>
{
    private readonly FinContext _context;
    //private readonly IStockOpnameCacheRemover _cacheRemover;

    public StockOpnameInsertHandler(FinContext db/*, IStockOpnameCacheRemover cacheRemover*/)
    {
        _context = db;
        //_cacheRemover = cacheRemover;
    }

    public async Task<PagedListResponse<StockProductListDto>> Handle(StockOpnameInsertList request, CancellationToken cancellationToken)
    {
        var result = new PagedListResponse<StockProductListDto>();

        var ItemStock = new List<ItemStock>();

        try
        {
            //await _context.ItemStock.AddAsync(ItemStock, cancellationToken);
            //await _context.SaveChangesAsync(cancellationToken);

            //_cacheRemover.RemoveCache();

            var row = new List<StockProductListDto>();
            foreach (StockOpnameInsert Item in request.StockOpnameList)
            {
                ItemStock.Add(new ItemStock
                {
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

            await _context.ItemStock.AddRangeAsync(ItemStock, cancellationToken);
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

public class StockOpnameInsertList : IRequest<PagedListResponse<StockProductListDto>>
{
    public List<StockOpnameInsert> StockOpnameList { get; set; }
}

public class StockOpnameInsert
{
    public int WarehouseId { get; set; } = 0;
    public int ItemId { get; set; } = 0;
    public double Quantity { get; set; } = 0;
    public double Cost { get; set; } = 0;
    public double Price { get; set; } = 0;
}
//public class StockOpnameInsertValidator : AbstractValidator<StockOpnameInsertList>
//{
//    private readonly IUniqueNameValidationRepository _repository;

//    public StockOpnameInsertValidator(IUniqueNameValidationRepository repository)
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
