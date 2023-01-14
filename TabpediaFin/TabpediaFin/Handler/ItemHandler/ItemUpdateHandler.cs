using TabpediaFin.Handler.ItemItemCategoryHandler;
using TabpediaFin.Handler.ItemUnitMeasureHandler;

namespace TabpediaFin.Handler.Item;

public class ItemUpdateHandler : IRequestHandler<ItemUpdateDto, RowResponse<ItemDto>>
{
    private readonly DbManager _dbManager;
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ItemUpdateHandler(FinContext db, ICurrentUser currentUser, DbManager dbManager)
    {
        _context = db;
        _currentUser = currentUser;
        _dbManager = dbManager;
    }

    public async Task<RowResponse<ItemDto>> Handle(ItemUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemDto>();
        int itemIdResult;
        List<ItemItemCategory> itemItemCategory = new List<ItemItemCategory>();
        List<ItemUnitMeasure> itemUnitMeasure = new List<ItemUnitMeasure>();
        List<ItemAttachment> itemAttachment = new List<ItemAttachment>();

        List<ItemItemCategoryFetchDto> itemItemCategoryFetchDto = new List<ItemItemCategoryFetchDto>();
        List<ItemUnitMeasureFetchDto> itemUnitMeasureFetchDto = new List<ItemUnitMeasureFetchDto>();
        List<ItemFetchAttachment> itemFetchAttachment = new List<ItemFetchAttachment>();

        try
        {
            var item = await _context.Item.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            item.Name = request.Name;
            item.Description = request.Description;
            item.Code = request.Code;
            item.Barcode = request.Barcode;
            item.UnitMeasureId = request.UnitMeasureId;
            item.AverageCost = request.AverageCost;
            item.Cost = request.Cost;
            item.Price = request.Price;
            item.IsSales = request.IsSales;
            item.IsPurchase = request.IsPurchase;
            item.IsStock = request.IsStock;
            item.StockMin = request.StockMin;
            item.IsArchived = request.IsArchived;
            item.ImageFileName = request.ImageFileName;
            item.Notes = request.Notes;
            item.PurchaseAccount = request.PurchaseAccount;
            item.SalesAccount = request.SalesAccount;


            itemIdResult = request.Id;
            List<int> idUpdateItemItemCategory = new List<int>();
            List<int> idUpdateUnitMeasure = new List<int>();
            List<int> idUpdateAttachment = new List<int>();
            if (request.ItemItemCategoryList.Count > 0)
            {
                foreach (ItemItemCategoryUpdate i in request.ItemItemCategoryList)
                {
                    idUpdateItemItemCategory.Add(i.Id);
                    itemItemCategory.Add(new ItemItemCategory
                    {
                        Id = i.Id,
                        ItemId = itemIdResult,
                        ItemCategoryId = i.ItemCategoryId,
                        CreatedUid = _currentUser.UserId,
                    });
                    itemItemCategoryFetchDto.Add(new ItemItemCategoryFetchDto
                    {
                        Id = i.Id,
                        ItemId = itemIdResult,
                        ItemCategoryId = i.ItemCategoryId,
                    });
                }
                _context.ItemItemCategory.UpdateRange(itemItemCategory);
            }

            if (request.ItemUnitMeasureList.Count > 0)
            {
                foreach (ItemUnitMeasureUpdate i in request.ItemUnitMeasureList)
                {
                    idUpdateUnitMeasure.Add(i.Id);
                    itemUnitMeasure.Add(new ItemUnitMeasure
                    {
                        Id = i.Id,
                        UnitMeasureId = i.UnitMeasureId,
                        ItemId = itemIdResult,
                        UnitConversion = i.UnitConversion,
                        Cost = i.Cost,
                        Price = i.Price,
                        Notes = i.Notes,
                        CreatedUid = _currentUser.UserId,
                    });
                    itemUnitMeasureFetchDto.Add(new ItemUnitMeasureFetchDto
                    {
                        Id = item.Id,
                        UnitMeasureId = i.UnitMeasureId,
                        ItemId = itemIdResult,
                        UnitConversion = i.UnitConversion,
                        Cost = i.Cost,
                        Price = i.Price,
                        Notes = i.Notes,
                    });
                }
                _context.ItemUnitMeasure.UpdateRange(itemUnitMeasure);
            }

            if (request.AttachmentFile.Count > 0)
            {
                foreach (ItemAttahmentUpdate i in request.AttachmentFile)
                {
                    idUpdateAttachment.Add(i.Id);
                    itemAttachment.Add(new ItemAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        CreatedUid = _currentUser.UserId,
                        ItemId = itemIdResult
                    });
                    itemFetchAttachment.Add(new ItemFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        ItemId = itemIdResult
                    });
                }
                _context.ItemAttachment.UpdateRange(itemAttachment);
            }


            List<ItemItemCategory> itemItemCategoryList = _context.ItemItemCategory.Where<ItemItemCategory>(x => x.ItemId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateItemItemCategory.Contains(x.Id)).ToList();
            List<ItemUnitMeasure> itemUnitMeasureList = _context.ItemUnitMeasure.Where<ItemUnitMeasure>(x => x.ItemId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateUnitMeasure.Contains(x.Id)).ToList();
            List<ItemAttachment> itemAttachmentList = _context.ItemAttachment.Where<ItemAttachment>(x => x.ItemId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateAttachment.Contains(x.Id)).ToList();
            _context.ItemItemCategory.RemoveRange(itemItemCategoryList);
            _context.ItemUnitMeasure.RemoveRange(itemUnitMeasureList);
            _context.ItemAttachment.RemoveRange(itemAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ItemDto()
            {
                Id = request.Id,
                Name = item.Name,
                Description = item.Description,
                Code = item.Code,
                Barcode = item.Barcode,
                UnitMeasureId = item.UnitMeasureId,
                AverageCost = item.AverageCost,
                Cost = item.Cost,
                Price = item.Price,
                IsSales = item.IsSales,
                IsPurchase = item.IsPurchase,
                IsStock = item.IsStock,
                StockMin = item.StockMin,
                IsArchived = item.IsArchived,
                ImageFileName = item.ImageFileName,
                Notes = item.Notes,
                ItemItemCategoryList = itemItemCategoryFetchDto,
                ItemUnitMeasureList = itemUnitMeasureFetchDto,
                ItemAttachmentList = itemFetchAttachment,
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
    public int PurchaseAccount { get; set; } = 0;
    public int SalesAccount { get; set; } = 0;
    public List<ItemItemCategoryUpdate> ItemItemCategoryList { get; set; }
    public List<ItemUnitMeasureUpdate> ItemUnitMeasureList { get; set; }
    public List<ItemAttahmentUpdate> AttachmentFile { get; set; }
}
public class ItemItemCategoryUpdate
{
    public int Id { get; set; } = 0;
    public int ItemId { get; set; } = 0;
    public int ItemCategoryId { get; set; } = 0;
}
public class ItemUnitMeasureUpdate
{
    public int Id { get; set; } = 0;
    public int UnitMeasureId { get; set; } = 0;
    public int ItemId { get; set; } = 0;
    public int UnitConversion { get; set; } = 0;
    public int Cost { get; set; } = 0;
    public int Price { get; set; } = 0;
    public string Notes { get; set; } = string.Empty;
}
public class ItemAttahmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int ItemId { get; set; }
}