using TabpediaFin.Handler.ItemItemCategoryHandler;
using TabpediaFin.Handler.ItemUnitMeasureHandler;

namespace TabpediaFin.Handler.Item;
public class ItemInsertHandler : IRequestHandler<ItemInsertDto, RowResponse<ItemDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ItemInsertHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }


    public async Task<RowResponse<ItemDto>> Handle(ItemInsertDto req, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ItemDto>();
        int itemIdResult;
        int itemUnitMeasureIdResult;

        var item = new Domain.Item()
        {
            Name = req.Name,
            Description = req.Description,
            Code = req.Code,
            Barcode = req.Barcode,
            UnitMeasureId = req.UnitMeasureId,
            AverageCost = req.AverageCost,
            Cost = req.Cost,
            Price = req.Price,
            IsSales = req.IsSales,
            IsPurchase = req.IsPurchase,
            IsStock = req.IsStock,
            StockMin = req.StockMin,
            IsArchived = req.IsArchived,
            ImageFileName = req.ImageFileName,
            Notes = req.Notes,
            PurchaseAccount = req.PurchaseAccount,
            SalesAccount = req.SalesAccount,
            PurchaseTax = req.PurchaseTax,
            SalesTax = req.SalesTax,


        };
        List<ItemItemCategory> itemItemCategory = new List<ItemItemCategory>();
        List<ItemUnitMeasure> itemUnitMeasure = new List<ItemUnitMeasure>();
        List<ItemItemCategoryFetchDto> itemItemCategoryFetchDto = new List<ItemItemCategoryFetchDto>();
        List<ItemUnitMeasureFetchDto> itemUnitMeasureFetchDto = new List<ItemUnitMeasureFetchDto>();
        

        try
        {
            await _context.Item.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            itemIdResult = item.Id;
            itemUnitMeasureIdResult = item.UnitMeasureId;

            List<ItemFetchAttachment> returnfile = await PostAttachmentAsync(req.ItemAttachmentFile, itemIdResult, cancellationToken);


            if (req.ItemItemCategoryList.Count > 0)
            {
                foreach (ItemItemCategoryInsertDto itm in req.ItemItemCategoryList)
                {
                    itemItemCategory.Add(new ItemItemCategory
                    {
                        ItemId = itemIdResult,
                        ItemCategoryId = itm.ItemCategoryId
                    });
                    itemItemCategoryFetchDto.Add(new ItemItemCategoryFetchDto
                    {
                        ItemId = itemIdResult,
                        ItemCategoryId = itm.ItemCategoryId,
                        Id = itm.Id
                    });
                }
                await _context.ItemItemCategory.AddRangeAsync(itemItemCategory, cancellationToken);
            }
            if (req.ItemUnitMeasureList.Count > 0)
            {
                foreach (ItemUnitMeasureInsertDto itm in req.ItemUnitMeasureList)
                {
                    itemUnitMeasure.Add(new ItemUnitMeasure
                    {
                        UnitMeasureId = itemUnitMeasureIdResult,
                        ItemId = itemIdResult,
                        UnitConversion = itm.UnitConversion,
                        Cost = itm.Cost,
                        Price = itm.Price,
                        Notes = itm.Notes
                    });
                    itemUnitMeasureFetchDto.Add(new ItemUnitMeasureFetchDto
                    {
                        UnitMeasureId = itemUnitMeasureIdResult,
                        ItemId = itemIdResult,
                        UnitConversion = itm.UnitConversion,
                        Cost = itm.Cost,
                        Price = itm.Price,
                        Notes = itm.Notes,
                        Id = itm.Id
                    });
                }
                await _context.ItemUnitMeasure.AddRangeAsync(itemUnitMeasure, cancellationToken);
            }
            if (itemUnitMeasure.Count > 0 || itemItemCategory.Count > 0)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
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
                PurchaseAccount = item.PurchaseAccount,
                SalesAccount = item.SalesAccount,
                PurchaseTax = item.PurchaseTax,
                SalesTax = item.SalesTax,
                ItemItemCategoryList = itemItemCategoryFetchDto,
                ItemUnitMeasureList = itemUnitMeasureFetchDto,

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

    public async Task<List<ItemFetchAttachment>> PostAttachmentAsync(List<ItemAttahmentFiles> filedata, int itemIdResult, CancellationToken cancellationToken)
    {
        List<ItemAttachment> ItemAttachmentList = new List<ItemAttachment>();
        List<ItemFetchAttachment> ItemFetchAttachmentList = new List<ItemFetchAttachment>();

        if (filedata.Count > 0)
        {
            foreach (ItemAttahmentFiles item in filedata)
            {
                ItemAttachmentList.Add(new ItemAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    ItemId = itemIdResult,
                });
                ItemFetchAttachmentList.Add(new ItemFetchAttachment
                {
                    FileName = item.FileName,
                    FileUrl = item.FileUrl,
                    Extension = item.Extension,
                    FileSize = item.FileSize,
                    ItemId = itemIdResult,
                });
            }

            await _context.ItemAttachment.AddRangeAsync(ItemAttachmentList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ItemFetchAttachmentList;
    }

}



public class ItemInsertDto : IRequest<RowResponse<ItemDto>>
{
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
    public int PurchaseTax { get; set; } = 0;
    public int SalesAccount { get; set; } = 0;
    public int SalesTax { get; set; } = 0;
    public List<ItemItemCategoryInsertDto> ItemItemCategoryList { get; set; }
    public List<ItemUnitMeasureInsertDto> ItemUnitMeasureList { get; set; }
    public List<ItemAttahmentFiles> ItemAttachmentFile { get; set; }
}

public class ItemAttahmentFiles
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    //public int ItemId { get; set; }
}