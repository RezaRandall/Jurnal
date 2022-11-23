using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
//using OfficeOpenXml;

namespace TabpediaFin.Handler.ExportImportContactHandler
{
    public class ContactReadFileHandler : IQueryReadFileExcelHandler<ContactReadFileListDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public ContactReadFileHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<PagedListResponse<ContactReadFileListDto>> Handle(QueryReadFileExcelDto<ContactReadFileListDto> request, CancellationToken cancellationToken)
        {
            var result = new PagedListResponse<ContactReadFileListDto>();

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
                var list = new List<ContactReadFileListDto>();
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
                            list.Add(new ContactReadFileListDto
                            {
                                Name = sheetRow.GetCell(0).StringCellValue,
                                Address = sheetRow.GetCell(1).StringCellValue,
                                CityName = sheetRow.GetCell(2).StringCellValue,
                                PostalCode = sheetRow.GetCell(3).StringCellValue,
                                Email = sheetRow.GetCell(4).StringCellValue,
                                Phone = sheetRow.GetCell(5).StringCellValue,
                                Fax = sheetRow.GetCell(6).StringCellValue,
                                Website = sheetRow.GetCell(7).StringCellValue,
                                Npwp = sheetRow.GetCell(8).StringCellValue,
                                //GroupId = int.Parse(sheetRow.GetCell(9).StringCellValue),
                                IsCustomer = bool.Parse(sheetRow.GetCell(10).StringCellValue),
                                IsVendor = bool.Parse(sheetRow.GetCell(11).StringCellValue),
                                IsEmployee = bool.Parse(sheetRow.GetCell(12).StringCellValue),
                                IsOther = bool.Parse(sheetRow.GetCell(13).StringCellValue),
                                Notes = sheetRow.GetCell(0).StringCellValue,
                            });
                        }
                    }
                }

                int recordCount = list.Count();

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list?.AsList() ?? new List<ContactReadFileListDto>();
                result.RecordCount = recordCount;

                //var list = new List<ContactReadFileListDto>();

                //using (var stream = new MemoryStream())
                //{
                //    await request.formFile.CopyToAsync(stream, cancellationToken);

                //    using (var package = new ExcelPackage(stream))
                //    {
                //        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                //        var rowCount = worksheet.Dimension.Rows;

                //        for (int row = 2; row <= rowCount; row++)
                //        {
                //            list.Add(new ContactReadFileListDto
                //            {
                //                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                //                Address = worksheet.Cells[row, 2].Value.ToString().Trim(),
                //                CityName = worksheet.Cells[row, 3].Value.ToString().Trim(),
                //                PostalCode = worksheet.Cells[row, 4].Value.ToString().Trim(),
                //                Email = worksheet.Cells[row, 5].Value.ToString().Trim(),
                //                Phone = worksheet.Cells[row, 6].Value.ToString().Trim(),
                //                Fax = worksheet.Cells[row, 7].Value.ToString().Trim(),
                //                Website = worksheet.Cells[row, 8].Value.ToString().Trim(),
                //                Npwp = worksheet.Cells[row, 9].Value.ToString().Trim(),
                //                GroupId = int.Parse(worksheet.Cells[row, 10].Value.ToString().Trim()),
                //                IsCustomer = bool.Parse(worksheet.Cells[row, 11].Value.ToString().Trim()),
                //                IsVendor = bool.Parse(worksheet.Cells[row, 12].Value.ToString().Trim()),
                //                IsEmployee = bool.Parse(worksheet.Cells[row, 13].Value.ToString().Trim()),
                //                IsOther = bool.Parse(worksheet.Cells[row, 14].Value.ToString().Trim()),
                //                Notes = worksheet.Cells[row, 15].Value.ToString().Trim(),
                //            });
                //        }
                //    }

                //    int recordCount = list.Count();

                //    result.IsOk = true;
                //    result.ErrorMessage = string.Empty;
                //    result.List = list?.AsList() ?? new List<ContactReadFileListDto>();
                //    result.RecordCount = recordCount;
                //}
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }


    public class ContactReadFileListDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Npwp { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsVendor { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsOther { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
