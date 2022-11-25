using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
//using OfficeOpenXml;
using TabpediaFin.Handler.ContactHandler;
using TabpediaFin.Handler.ExportImportContactHandler;

namespace TabpediaFin.Controllers
{
    [Route("api/exporimporcontact")]
    [ApiController]
    public class ExportImportContactController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public ExportImportContactController(IMediator mediator, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _mediator = mediator;
            this._hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("filestemplate")]
        public async Task<ActionResult> DownloadFile()
        {
            var filePath = $"TemplateImport/Template_Contact_Import.xlsx";

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(filePath));
        }

        [HttpPost("readexcel")]
        public async Task<IActionResult> Read(IFormFile formFile)
        {
            QueryReadFileExcelDto<ContactReadFileListDto> request = new QueryReadFileExcelDto<ContactReadFileListDto>();
            request.formFile = formFile;
            return Result(await _mediator.Send(request));
        }
        [HttpPost("import")]
        public async Task<IActionResult> Impor(ContactImportInsertListDto command)
        {
            return Result(await _mediator.Send(command));
        }
        //[HttpGet("export")]
        //public async Task<IActionResult> Export(CancellationToken cancellationToken)
        //{
        //    QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
        //    reqsend.PageSize = int.MaxValue;
        //    reqsend.PageNum = 0;
        //    reqsend.Search = "";
        //    reqsend.SortBy = "";
        //    reqsend.SortDesc = false;
        //    var result = await _mediator.Send(reqsend);
        //    await Task.Yield();
        //    var list = new List<contactlistDto>();
        //    list = result.List;
        //    var stream = new MemoryStream();

        //    using (var package = new ExcelPackage(stream))
        //    {
        //        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
        //        workSheet.Cells.LoadFromCollection(list, true);
        //        package.Save();
        //    }
        //    stream.Position = 0;
        //    string excelName = $"ContactList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

        //    //return File(stream, "application/octet-stream", excelName);  
        //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //}

        //[HttpGet("exportv2")]
        //public async Task<DemoResponse<string>> Exportv2(CancellationToken cancellationToken)
        //{
        //    string folder = "../TabpediaFin/excelexport";
        //    string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
        //    string downloadUrl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, excelName);
        //    FileInfo file = new FileInfo(Path.Combine(folder, excelName));
        //    if (file.Exists)
        //    {
        //        file.Delete();
        //        file = new FileInfo(Path.Combine(folder, excelName));
        //    }

        //    QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
        //    reqsend.PageSize = int.MaxValue;
        //    reqsend.PageNum = 0;
        //    reqsend.Search = "";
        //    reqsend.SortBy = "";
        //    reqsend.SortDesc = false;
        //    var result = await _mediator.Send(reqsend);
        //    await Task.Yield();
        //    var list = new List<contactlistDto>();
        //    list = result.List;
            
        //    await Task.Yield();

        //    using (var package = new ExcelPackage(file))
        //    {
        //        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
        //        workSheet.Cells.LoadFromCollection(list, true);
        //        package.Save();
        //    }

        //    return DemoResponse<string>.GetResult(0, "OK", downloadUrl);
        //}

        [HttpPost("export")]
        public async Task<IActionResult> ExportAllToExcelAsync()
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("sheet1");
            
            QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
            reqsend.PageSize = int.MaxValue;
            reqsend.PageNum = 0;
            reqsend.Search = "";
            reqsend.SortBy = "";
            reqsend.SortDesc = false;
            var result = await _mediator.Send(reqsend);
            
            var list = new List<contactlistDto>();
            list = result.List;

            var properties = new[] { "Id", "Name", "Address", "CityName", "PostalCode", "Email", "Phone", "Fax", "Website", "Npwp", "groupName", "GroupId", "IsCustomer", "IsVendor", "IsEmployee", "IsOther", "Notes"};
            var headers = new[] { "Id", "Name", "Address", "CityName", "PostalCode", "Email", "Phone", "Fax", "Website", "Npwp", "groupName", "GroupId", "IsCustomer", "IsVendor", "IsEmployee", "IsOther", "Notes" };

            var headerRow = sheet.CreateRow(0);

            // create header
            for (int i = 0; i < properties.Length; i++)
            {
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(headers[i]);
            }

            // fill content 
            for (int i = 0; i < list.Count; i++)
            {
                var rowIndex = i + 1;
                var row = sheet.CreateRow(rowIndex);

                for (int j = 0; j < properties.Length; j++)
                {
                    var cell = row.CreateCell(j);
                    var o = list[i];
                    cell.SetCellValue(o.GetType().GetProperty(properties[j]).GetValue(o, null).ToString());
                }
            }

            using (var stream = new MemoryStream())
            {
                workbook.Write(stream);
                stream.Close();
                return File(stream.ToArray(), "application/vnd.ms-excel", $"ContactList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xls");
            }
        }


        //public class DemoResponse<T>
        //{
        //    public int Code { get; set; }

        //    public string Msg { get; set; }

        //    public T Data { get; set; }

        //    public static DemoResponse<T> GetResult(int code, string msg, T data = default(T))
        //    {
        //        return new DemoResponse<T>
        //        {
        //            Code = code,
        //            Msg = msg,
        //            Data = data
        //        };
        //    }
        //}

    }
}
