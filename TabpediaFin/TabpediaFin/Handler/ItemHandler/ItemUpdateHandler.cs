using Org.BouncyCastle.Asn1.Ocsp;
using TabpediaFin.Handler.ItemItemCategoryHandler;
using TabpediaFin.Handler.ItemUnitMeasureHandler;

namespace TabpediaFin.Handler.Item;

public class ItemUpdateHandler : IRequestHandler<ItemUpdateDto, RowResponse<ItemDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ItemUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ItemDto>> Handle(ItemUpdateDto req, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemDto>();
        int itemIdResult;
        int itemUnitMeasureIdResult;

        List<ItemItemCategory> itemItemCategory = new List<ItemItemCategory>();
        List<ItemUnitMeasure> itemUnitMeasure = new List<ItemUnitMeasure>();
        List<ItemItemCategoryFetchDto> itemItemCategoryFetchDto = new List<ItemItemCategoryFetchDto>();
        List<ItemUnitMeasureFetchDto> itemUnitMeasureFetchDto = new List<ItemUnitMeasureFetchDto>();

        List<ItemFetchAttachment> ItemFetchAttachment = new List<ItemFetchAttachment>();
        try
        {
            var item = await _context.Item.FirstAsync(x => x.Id == req.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
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
            itemIdResult = req.Id;
            itemUnitMeasureIdResult = req.UnitMeasureId;

            List<ItemFetchAttachment> returnfile = await UpdateAttachmentAsync(req.AttachmentFile, itemIdResult, cancellationToken);

            var row = new ItemDto()
            {
                Id = item.Id,
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
                ItemUnitMeasureList = itemUnitMeasureFetchDto
            };

            if (req.ItemItemCategoryList.Count > 0)
            {
                foreach (ItemItemCategoryUpdateDto itm in req.ItemItemCategoryList)
                {
                    itemItemCategory.Add(new ItemItemCategory
                    {
                        Id = itm.Id,
                        ItemId = itemIdResult,
                        ItemCategoryId = itm.ItemCategoryId
                    });
                    itemItemCategoryFetchDto.Add(new ItemItemCategoryFetchDto
                    {
                        Id = itm.Id,
                        ItemId = itemIdResult,
                        ItemCategoryId = itm.ItemCategoryId
                    });
                }
                _context.ItemItemCategory.UpdateRange(itemItemCategory);
            }
            if (req.ItemUnitMeasureList.Count > 0)
            {
                foreach (ItemUnitMeasureUpdateDto itm in req.ItemUnitMeasureList)
                {
                    itemUnitMeasure.Add(new ItemUnitMeasure
                    {
                        Id = itm.Id,
                        UnitMeasureId = itemUnitMeasureIdResult,
                        ItemId = itemIdResult,
                        UnitConversion = itm.UnitConversion,
                        Cost = itm.Cost,
                        Price = itm.Price,
                        Notes = itm.Notes
                    });
                    itemUnitMeasureFetchDto.Add(new ItemUnitMeasureFetchDto
                    {
                        Id = itm.Id,
                        UnitMeasureId = itemUnitMeasureIdResult,
                        ItemId = itemIdResult,
                        UnitConversion = itm.UnitConversion,
                        Cost = itm.Cost,
                        Price = itm.Price,
                        Notes = itm.Notes
                    });
                }
                _context.ItemUnitMeasure.UpdateRange(itemUnitMeasure);
            }
            if (itemUnitMeasure.Count > 0 || itemItemCategory.Count > 0)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
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

    public async Task<List<ItemFetchAttachment>> UpdateAttachmentAsync(List<ItemAttahmentUpdate> filedata, int TransId, CancellationToken cancellationToken)
    {
        List<ItemAttachment> ItemAttachmentList = new List<ItemAttachment>();
        List<ItemFetchAttachment> ItemFetchAttachmentList = new List<ItemFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (ItemAttahmentUpdate item in filedata)
            {
                ItemAttachmentList.Add(new ItemAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    ItemId = TransId,
                });
                ItemFetchAttachmentList.Add(new ItemFetchAttachment
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    ItemId = TransId,
                });
            }

            _context.ItemAttachment.UpdateRange(ItemAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ItemFetchAttachmentList;
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
    public List<ItemItemCategoryUpdateDto> ItemItemCategoryList { get; set; }
    public List<ItemUnitMeasureUpdateDto> ItemUnitMeasureList { get; set; }
    public List<ItemAttahmentUpdate> AttachmentFile { get; set; }
}

public class ItemAttahmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
}