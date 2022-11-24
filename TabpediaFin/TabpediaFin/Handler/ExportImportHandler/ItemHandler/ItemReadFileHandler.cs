using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace TabpediaFin.Handler.ExportImportItemHandler
{
    public class ItemReadFileHandler : IQueryReadFileExcelHandler<ItemReadFileListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ItemReadFileHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<ItemReadFileListDto>> Handle(QueryReadFileExcelDto<ItemReadFileListDto> request, CancellationToken cancellationToken)
        {
            var result = new PagedListResponse<ItemReadFileListDto>();

            try
            {
                if (request.formFile == null || request.formFile.Length <= 0)
                {
                    result.IsOk = false;
                    result.ErrorMessage = "file is empty";
                    return result;
                }

                if (!Path.GetExtension(request.formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    result.IsOk = false;
                    result.ErrorMessage = "Not Support file extension";
                    return result;
                }
                DataTable dtTable = new DataTable();
                List<string> rowList = new List<string>();
                ISheet sheet;
                byte[] bytes;
                var list = new List<ItemReadFileListDto>();
                using (var ms = new MemoryStream())
                {
                    request.formFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    bytes = Convert.FromBase64String(s);
                }
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    NPOI.XSSF.UserModel.XSSFWorkbook hSSFWorkbook = new NPOI.XSSF.UserModel.XSSFWorkbook(ms);
                    sheet = hSSFWorkbook.GetSheetAt(0);
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        var sheetRow = sheet.GetRow(row);
                        if (sheetRow != null)
                        {
                            list.Add(new ItemReadFileListDto
                            {
                                Name = sheetRow.GetCell(0).StringCellValue,
                                Description =  sheetRow.GetCell(1).StringCellValue,
                                Code =  sheetRow.GetCell(2).StringCellValue,
                                Barcode =  sheetRow.GetCell(3).StringCellValue,
                                UnitMeasureId = int.Parse(sheetRow.GetCell(4).StringCellValue),
                                AverageCost =  int.Parse(sheetRow.GetCell(5).StringCellValue),
                                Cost =  int.Parse(sheetRow.GetCell(6).StringCellValue),
                                Price =  int.Parse(sheetRow.GetCell(7).StringCellValue),
                                IsSales = bool.Parse(sheetRow.GetCell(8).StringCellValue),
                                IsPurchase = bool.Parse(sheetRow.GetCell(9).StringCellValue),
                                IsStock =  bool.Parse(sheetRow.GetCell(10).StringCellValue),
                                StockMin =  int.Parse(sheetRow.GetCell(11).StringCellValue),
                                IsArchived =  bool.Parse(sheetRow.GetCell(12).StringCellValue),
                                ImageFileName =  sheetRow.GetCell(13).StringCellValue,
                                Notes =  sheetRow.GetCell(14).StringCellValue,

                            });
                        }
                    }
                }

                int recordCount = list.Count();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list?.AsList() ?? new List<ItemReadFileListDto>();
                result.RecordCount = recordCount;
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }


    public class ItemReadFileListDto
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
    }
}
