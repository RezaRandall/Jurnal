namespace TabpediaFin.Handler.PurchaseOrderHandler
{
    public class PurchaseOrderFetchHandlerI : IFetchByIdHandler<PurchaseOrderFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public PurchaseOrderFetchHandlerI(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseOrderFetchDto>> Handle(FetchByIdRequestDto<PurchaseOrderFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<PurchaseOrderFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sql = @"SELECT  c.""Name"" as Vendor, 
                                        a.""FullName"" as Staff, 
                                        i.""Id"", 
                                        i.""TenantId"",
                                        i.""StaffId"", 
                                        i.""VendorId"", 
                                        i.""WarehouseId"",
                                        i.""TransCode"", 
                                        i.""TransDate"",
                                        i.""DueDate"",
                                        CASE 
                                            WHEN i.""DueDate"" < now()  AND i.""Status"" = 0
                                                THEN 3
                                            ELSE
                                                i.""Status""
                                            END AS Status,
                                        CASE 
                                            WHEN i.""DueDate"" >= now()  AND i.""Status"" = 0
                                                THEN 'Open'
                                            WHEN i.""DueDate"" < now()  AND i.""Status"" = 0
                                                THEN 'Expired'
                                            ELSE
                                                'Closed'
                                            END AS StatusString,
                                        i.""Memo"",
                                        i.""Notes""  
                                        FROM ""PurchaseOrder"" i LEFT JOIN ""AppUser"" a ON a.""Id"" = i.""StaffId"" LEFT JOIN ""Contact"" c ON c.""Id"" = i.""VendorId"" WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @Id";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("Id", request.Id);
                    var result = await cn.QueryFirstOrDefaultAsync<PurchaseOrderFetchDto>(sql, parameters);

                    if (result != null)
                    {
                        var parametersub = new DynamicParameters();
                        parametersub.Add("TenantId", _currentUser.TenantId);
                        parametersub.Add("TransId", request.Id);

                        var sqltag = @"SELECT at.""Name"" as TagName, at.""Description"" as TagDescription, i.""Id"", i.""TagId"", i.""TransId"" FROM ""PurchaseOrderTag"" i LEFT JOIN ""Tag"" at ON i.""TagId"" = at.""Id"" WHERE i.""TenantId"" = @TenantId AND i.""TransId"" = @TransId";

                        List<PurchaseOrderFetchTag> resultaddress;
                        resultaddress = (await cn.QueryAsync<PurchaseOrderFetchTag>(sqltag, parametersub).ConfigureAwait(false)).ToList();

                        result.TagList = resultaddress; 

                        var sqlattachment = @"SELECT i.""FileName"", i.""FileUrl"", i.""Extension"", i.""FileSize"", i.""TransId"",i.""Id""  FROM ""PurchaseOrderAttachment"" i WHERE i.""TenantId"" = @TenantId AND i.""TransId"" = @TransId";

                        List<PurchaseOrderFetchAttachment> resultattachment;
                        resultattachment = (await cn.QueryAsync<PurchaseOrderFetchAttachment>(sqlattachment, parametersub).ConfigureAwait(false)).ToList();

                        result.AttachmentList = resultattachment;

                        var sqlitem = @"SELECT It.""AverageCost"" as ItemAverageCost, It.""Cost"" as ItemCost, It.""Name"" as ItemName, It.""Description"" as ItemDescription, um.""Id"" as UnitMeasureId,um.""Name"" as UnitMeasureName, um.""Description"" as UnitMeasureDescription, i.""Id"", i.""ItemId"", i.""TransId"", i.""Quantity"", i.""ItemUnitMeasureId""  FROM ""PurchaseOrderItem"" i LEFT JOIN ""Item"" It ON i.""ItemId"" = It.""Id"" LEFT JOIN ""ItemUnitMeasure"" ium ON i.""ItemUnitMeasureId"" = ium.""Id"" LEFT JOIN ""UnitMeasure"" um ON ium.""UnitMeasureId"" = um.""Id"" WHERE i.""TenantId"" = @TenantId AND i.""TransId"" = @TransId";

                        List<PurchaseOrderFetchItem> resultitem;
                        resultitem = (await cn.QueryAsync<PurchaseOrderFetchItem>(sqlitem, parametersub).ConfigureAwait(false)).ToList();

                        result.ItemList = resultitem;
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

    public class PurchaseOrderFetchDto : BaseDto
    {
        public int StaffId { get; set; }
        public string Staff { get; set; }
        public int VendorId { get; set; }
        public int WarehouseId { get; set; }
        public string Vendor { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TransCode { get; set; } = string.Empty;
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int Status { get; set; }
        public string StatusString { get; set; } = string.Empty;
        public List<PurchaseOrderFetchTag> TagList { get; set; }
        public List<PurchaseOrderFetchItem> ItemList { get; set; }
        public List<PurchaseOrderFetchAttachment> AttachmentList { get; set; }
    }

    public class PurchaseOrderFetchTag : BaseDto
    {
        public int TagId { get; set; }
        public int TransId { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string TagDescription { get; set; } = string.Empty;
    }

    public class PurchaseOrderFetchItem : BaseDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int ItemUnitMeasureId { get; set; }
        public int UnitMeasureId { get; set; }
        public double ItemAverageCost { get; set; }
        public double ItemCost { get; set; }
        public string UnitMeasureName { get; set; } = string.Empty;
        public string UnitMeasureDescription { get; set; } = string.Empty;
        public int TransId { get; set; }
    }
    public class PurchaseOrderFetchAttachment : BaseDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public int TransId { get; set; }
    }
}
