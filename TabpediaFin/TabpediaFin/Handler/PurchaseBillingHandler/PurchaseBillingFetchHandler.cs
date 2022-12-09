namespace TabpediaFin.Handler.PurchaseBillingHandler
{
    public class PurchaseBillingFetchHandlerI : IFetchByIdHandler<PurchaseBillingFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public PurchaseBillingFetchHandlerI(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<RowResponse<PurchaseBillingFetchDto>> Handle(FetchByIdRequestDto<PurchaseBillingFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<PurchaseBillingFetchDto>();

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
                                        i.""TransCode"", 
                                        i.""TransDate"",
                                        i.""DueDate"",
                                        i.""BudgetYear"",
                                        i.""UrgentLevel"",
                                        CASE 
                                            WHEN i.""UrgentLevel""  = 0
                                                THEN 'Low'
                                            WHEN i.""UrgentLevel""  = 1
                                                THEN 'Moderate'
                                            ELSE
                                                'High'
                                            END AS UrgentLevelString,
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
                                        i.""Notes"",
                                        i.""DiscountType"",
                                        i.""DiscountAmount"",
                                        CASE 
                                            WHEN i.""DiscountType"" = 0
                                                THEN 'Precentage'
                                            ELSE
                                                'Nominal'
                                            END AS DiscountTypeString,
                                        FROM ""PurchaseBilling"" i LEFT JOIN ""AppUser"" a ON a.""Id"" = i.""StaffId"" LEFT JOIN ""Contact"" c ON c.""Id"" = i.""VendorId"" WHERE i.""TenantId"" = @TenantId AND i.""Id"" = @Id";

                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    parameters.Add("Id", request.Id);
                    var result = await cn.QueryFirstOrDefaultAsync<PurchaseBillingFetchDto>(sql, parameters);

                    if (result != null)
                    {
                        var parametersub = new DynamicParameters();
                        parametersub.Add("TenantId", _currentUser.TenantId);
                        parametersub.Add("TransId", request.Id);

                        var sqltag = @"SELECT at.""Name"" as TagName, at.""Description"" as TagDescription, i.""Id"", i.""TagId"", i.""TransId"" FROM ""PurchaseBillingTag"" i LEFT JOIN ""Tag"" at ON i.""TagId"" = at.""Id"" WHERE i.""TenantId"" = @TenantId AND i.""TransId"" = @TransId";

                        List<PurchaseBillingFetchTag> resultaddress;
                        resultaddress = (await cn.QueryAsync<PurchaseBillingFetchTag>(sqltag, parametersub).ConfigureAwait(false)).ToList();

                        result.TagList = resultaddress; 

                        var sqlattachment = @"SELECT i.""FileName"", i.""FileUrl"", i.""Extension"", i.""FileSize"", i.""TransId"",i.""Id""  FROM ""PurchaseBillingAttachment"" i WHERE i.""TenantId"" = @TenantId AND i.""TransId"" = @TransId";

                        List<PurchaseBillingFetchAttachment> resultattachment;
                        resultattachment = (await cn.QueryAsync<PurchaseBillingFetchAttachment>(sqlattachment, parametersub).ConfigureAwait(false)).ToList();

                        result.AttachmentList = resultattachment;

                        var sqlitem = @"SELECT It.""AverageCost"" as ItemAverageCost, It.""Cost"" as ItemCost, It.""Name"" as ItemName, It.""Description"" as ItemDescription, um.""Id"" as UnitMeasureId,um.""Name"" as UnitMeasureName, um.""Description"" as UnitMeasureDescription, i.""Id"", i.""ItemId"", i.""TransId"", i.""Quantity"", i.""ItemUnitMeasureId""  FROM ""PurchaseBillingItem"" i LEFT JOIN ""Item"" It ON i.""ItemId"" = It.""Id"" LEFT JOIN ""ItemUnitMeasure"" ium ON i.""ItemUnitMeasureId"" = ium.""Id"" LEFT JOIN ""UnitMeasure"" um ON ium.""UnitMeasureId"" = um.""Id"" WHERE i.""TenantId"" = @TenantId AND i.""TransId"" = @TransId";

                        List<PurchaseBillingFetchItem> resultitem;
                        resultitem = (await cn.QueryAsync<PurchaseBillingFetchItem>(sqlitem, parametersub).ConfigureAwait(false)).ToList();

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

    public class PurchaseBillingFetchDto : BaseDto
    {
        public int StaffId { get; set; }
        public string Staff { get; set; }
        public int VendorId { get; set; }
        public string Vendor { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TransCode { get; set; } = string.Empty;
        public string BudgetYear { get; set; } = string.Empty;
        public int UrgentLevel { get; set; }
        public string UrgentLevelString { get; set; } = string.Empty;
        public string Memo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int Status { get; set; }
        public string StatusString { get; set; } = string.Empty;
        public int DiscountType { get; set; }
        public string DiscountTypeString { get; set; } = string.Empty;
        public double DiscountAmount { get; set; }
        public List<PurchaseBillingFetchTag> TagList { get; set; }
        public List<PurchaseBillingFetchItem> ItemList { get; set; }
        public List<PurchaseBillingFetchAttachment> AttachmentList { get; set; }
    }

    public class PurchaseBillingFetchTag : BaseDto
    {
        public int TagId { get; set; }
        public int TransId { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string TagDescription { get; set; } = string.Empty;
    }

    public class PurchaseBillingFetchItem : BaseDto
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
    public class PurchaseBillingFetchAttachment : BaseDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public int TransId { get; set; }
    }
}
