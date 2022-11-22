using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

namespace TabpediaFin.Handler.Item;

public class ItemUpdateHandler : IRequestHandler<ItemUpdateDto, RowResponse<ItemDto>>
{
    private readonly FinContext _context;

    public ItemUpdateHandler(FinContext db)
    {
        _context = db;
    }

    public class CommandValidator : AbstractValidator<ItemUpdateDto>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).MaximumLength(100); ;
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.Barcode).NotNull().NotEmpty();
            RuleFor(x => x.UnitMeasureId).NotNull().NotEmpty();
            RuleFor(x => x.AverageCost).NotNull().NotEmpty();
            RuleFor(x => x.Cost).NotNull().NotEmpty();
            RuleFor(x => x.Price).NotNull().NotEmpty();
            RuleFor(x => x.IsSales).NotNull().NotEmpty();
            RuleFor(x => x.IsPurchase).NotNull().NotEmpty();
            RuleFor(x => x.IsStock).NotNull().NotEmpty();
            RuleFor(x => x.StockMin).NotNull().NotEmpty();
            RuleFor(x => x.IsArchived).NotNull().NotEmpty();
            RuleFor(x => x.ImageFileName).NotNull().NotEmpty();
            RuleFor(x => x.Notes).MaximumLength(100);
        }
    }

    public async Task<RowResponse<ItemDto>> Handle(ItemUpdateDto req, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemDto>();

        try
        {
            var item = await _context.Item.FirstAsync(x => x.Id == req.Id, cancellationToken);
            item.Name = req.Name;
            item.Description = req.Description;
            item.Code = req.Code;
            item.Barcode = req.Barcode;
            item.UnitMeasureId = req.UnitMeasureId;
            item.AverageCost = req.AverageCost;
            item.Cost = req.Cost;
            item.Price = req.Price;
            item.IsSales = req.IsSales;
            item.IsPurchase = req.IsPurchase;
            item.IsStock = req.IsStock;
            item.StockMin = req.StockMin;
            item.IsArchived = req.IsArchived;
            item.ImageFileName = req.ImageFileName;
            item.Notes = req.Notes;

            await _context.SaveChangesAsync(cancellationToken);

            var row = new ItemDto()
            {
                Id = item.Id
                ,Name = item.Name
                ,Description = item.Description
                ,Code = item.Code
                ,Barcode = item.Barcode
                ,UnitMeasureId = item.UnitMeasureId
                ,AverageCost = item.AverageCost
                ,Cost = item.Cost
                ,Price = item.Price
                ,IsSales = item.IsSales
                ,IsPurchase = item.IsPurchase
                ,IsStock = item.IsStock
                ,StockMin = item.StockMin
                ,IsArchived = item.IsArchived
                ,ImageFileName = item.ImageFileName
                ,Notes = item.Notes
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

[Table("Item")]
public class ItemUpdateDto : IRequest<RowResponse<ItemDto>>
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public int UnitMeasureId { get; set; } = 0;
    public int AverageCost { get; set; } = 0;
    public int Cost { get; set; } = 0;
    public int Price { get; set; } = 0;
    public bool IsSales { get; set; } = true;
    public bool IsPurchase { get; set; } = true;
    public bool IsStock { get; set; } = true;
    public int StockMin { get; set; } = 0;
    public bool IsArchived { get; set; } = true;
    public string ImageFileName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
