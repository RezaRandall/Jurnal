using TabpediaFin.Domain;
using TabpediaFin.Handler.ContactHandler;
using TabpediaFin.Handler.ItemItemCategoryHandler;
using TabpediaFin.Handler.ItemUnitMeasureHandler;

namespace TabpediaFin.Handler.Item
{
    public class ItemFetchHandler : IFetchByIdHandler<ItemDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ItemFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<ItemDto>> Handle(FetchByIdRequestDto<ItemDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ItemDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sql = @"SELECT * FROM ""Item"" WHERE ""TenantId"" = @TenantId AND ""Id"" = @Id " ;

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("Id", request.Id);
                    var result = await cn.QueryFirstOrDefaultAsync<ItemDto>(sql, parameters);

                    if (result != null)
                    {
                        var sqlItemItemCategory = @"SELECT iic.""Id""
                                                ,iic.""ItemId""
                                                ,iic.""ItemCategoryId""
                                                 FROM ""ItemItemCategory"" iic
                                                 LEFT JOIN ""Item"" i ON iic.""ItemId"" = i.""Id"" 
                                                 WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @IdItem ";

                        var parametersub = new DynamicParameters();
                        parametersub.Add("TenantId", _currentUser.TenantId);
                        parametersub.Add("IdItem", request.Id);

                        List<ItemItemCategoryFetchDto> resultItemItemCategory;
                        resultItemItemCategory = (await cn.QueryAsync<ItemItemCategoryFetchDto>(sqlItemItemCategory, parametersub).ConfigureAwait(false)).ToList();
                        result.ItemItemCategoryList = resultItemItemCategory;

                        var sqlItemUnitMeasure = @"SELECT ium.""Id""
                                        ,ium.""ItemId""
                                        ,ium.""UnitMeasureId""
                                        ,ium.""UnitConversion""
                                        ,ium.""Cost""
                                        ,ium.""Price""
                                        ,ium.""Notes""
                                        FROM ""ItemUnitMeasure"" ium 
                                        INNER JOIN ""Item"" i ON ium.""ItemId"" = i.""Id""
                                        WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @IdItem";

                        List<ItemUnitMeasureFetchDto> resultItemUnitMeasure;
                        resultItemUnitMeasure = (await cn.QueryAsync<ItemUnitMeasureFetchDto>(sqlItemUnitMeasure, parametersub).ConfigureAwait(false)).ToList();
                        result.ItemUnitMeasureList = resultItemUnitMeasure;

                        var sqlAttachment = @"SELECT ia.""Id""
                                        , ia.""FileName""
                                        , ia.""FileUrl""
                                        , ia.""Extension""
                                        , ia.""FileSize""
                                        , ia.""ItemId"" 
                                        FROM ""ItemAttachment"" ia
                                        INNER JOIN ""Item"" i ON ia.""ItemId"" = i.""Id"" 
                                        WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @IdItem ";
                        List<ItemFetchAttachment> resultItemAttachment;
                        resultItemAttachment = (await cn.QueryAsync<ItemFetchAttachment>(sqlAttachment, parametersub).ConfigureAwait(false)).ToList();
                        result.ItemAttachmentList = resultItemAttachment;

                    }

                    response.IsOk = true;
                    response.Row = result;
                    response.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.IsOk = false;
                response.Row = null;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}

[Table("Item")]
public class ItemDto : BaseDto
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
    public List<ItemItemCategoryFetchDto> ItemItemCategoryList { get; set; }
    public List<ItemUnitMeasureFetchDto> ItemUnitMeasureList { get; set; }
    public List<ItemFetchAttachment> ItemAttachmentList { get; set; }
}

public class ItemFetchAttachment : BaseDto
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public int ItemId { get; set; }
}