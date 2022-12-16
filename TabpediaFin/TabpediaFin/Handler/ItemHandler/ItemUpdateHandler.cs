using NPOI.HPSF;
using TabpediaFin.Handler.ContactHandler;
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

            itemIdResult = request.Id;
            List<int> idUpdateItemItemCategory = new List<int>();
            List<int> idUpdateUnitMeasure = new List<int>();
            List<int> idUpdateAttachment = new List<int>();
            if (request.ItemItemCategoryList.Count > 0)
            {
                foreach (ItemItemCategoryUpdate itm in request.ItemItemCategoryList)
                {
                    idUpdateItemItemCategory.Add(item.Id);
                    itemItemCategory.Add(new ItemItemCategory
                    {
                        Id = itm.Id,
                        ItemId = itemIdResult,
                        ItemCategoryId = itm.ItemCategoryId,
                        UpdatedUid = _currentUser.UserId,
                    });
                    itemItemCategoryFetchDto.Add(new ItemItemCategoryFetchDto
                    {
                        Id = itm.Id,
                        ItemId = itemIdResult,
                        ItemCategoryId = itm.ItemCategoryId,
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
                        UnitMeasureId = itemIdResult,
                        ItemId = i.ItemId,
                        UnitConversion = i.UnitConversion,
                        Cost = i.Cost,
                        Price = i.Price,
                        Notes = i.Notes,
                        UpdatedUid = _currentUser.UserId,
                    });
                    itemUnitMeasureFetchDto.Add(new ItemUnitMeasureFetchDto
                    {
                        Id = item.Id,
                        UnitMeasureId = itemIdResult,
                        ItemId = i.ItemId,
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
                        UpdatedUid = _currentUser.UserId,
                    });
                    itemFetchAttachment.Add(new ItemFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
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
    public async Task<ItemDto> GetItem(int id)
    {
        ItemDto returnItem = new ItemDto();
        using (var cn = _dbManager.CreateConnection())
        {
            //var sql = @"SELECT c.""Name"" as groupName
            //        ,i.""Id""
            //        ,i.""TenantId""
            //        ,i.""Name""
            //        ,i.""Address""
            //        ,i.""CityName""
            //        ,i.""PostalCode""
            //        ,i.""Email""
            //        ,i.""Phone""
            //        ,i.""Fax""
            //        ,i.""Website""
            //        ,i.""Npwp""
            //        ,i.""GroupId""
            //        ,i.""Notes""
            //        ,i.""IsCustomer""
            //        ,i.""IsVendor""
            //        ,i.""IsEmployee""
            //        ,i.""IsOther""  
            //        FROM ""Contact"" i 
            //        LEFT JOIN ""ContactGroup"" c on i.""GroupId"" = c.""Id""  
            //        WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @Id";

            var sql = @"SELECT ""*""  
                    FROM ""Item""   
                    WHERE ""TenantId"" = @TenantId";

            var parameters = new DynamicParameters();
            parameters.Add("TenantId", _currentUser.TenantId);
            parameters.Add("Id", id);
            var result = await cn.QueryFirstOrDefaultAsync<ItemDto>(sql, parameters);

            if (result != null)
            {
                var sqladdress = @"SELECT ""*""
                    FROM ""ItemItemCategory""  
                    WHERE ""TenantId"" = @TenantId";

                var parametersub = new DynamicParameters();
                parametersub.Add("TenantId", _currentUser.TenantId);
                parametersub.Add("ItemId", id);

                List<ItemItemCategoryFetchDto> itemItemCategory;
                itemItemCategory = (await cn.QueryAsync<ItemItemCategoryFetchDto>(sqladdress, parametersub).ConfigureAwait(false)).ToList();

                result.ItemItemCategoryList = itemItemCategory;
                var sqlItemUnitMeasure = @"SELECT ""*""
                    FROM ""ItemUnitMeasure""  
                    WHERE ""TenantId"" = @TenantId";

                List<ItemUnitMeasureFetchDto> resultItemUnitMeasure;
                resultItemUnitMeasure = (await cn.QueryAsync<ItemUnitMeasureFetchDto>(sqlItemUnitMeasure, parametersub).ConfigureAwait(false)).ToList();

                result.ItemUnitMeasureList = resultItemUnitMeasure;
            }


            returnItem = result;
        }


        return returnItem;
    }

    public class IdComparer : IEqualityComparer<ItemItemCategory>
    {
        public int GetHashCode(ItemItemCategory co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.Id.GetHashCode();
        }

        public bool Equals(ItemItemCategory x1, ItemItemCategory x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.Id == x2.Id;
        }
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
}