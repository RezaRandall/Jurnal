using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
//using OfficeOpenXml;
using TabpediaFin.Handler.ExportImportItemHandler;
using TabpediaFin.Handler.Item;

namespace TabpediaFin.Controllers
{
    [Route("api/exporimporItem")]
    [ApiController]
    public class ExportImportItemController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public ExportImportItemController(IMediator mediator, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _mediator = mediator;
            this._hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("readexcel")]
        public async Task<IActionResult> Read(IFormFile formFile)
        {
            QueryReadFileExcelDto<ItemReadFileListDto> request = new QueryReadFileExcelDto<ItemReadFileListDto>();
            request.formFile = formFile;
            return Result(await _mediator.Send(request));
        }
        [HttpPost("import")]
        public async Task<IActionResult> Impor(ItemImportInsertListDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportAllToExcelAsync()
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("sheet1");
            
            QueryPagedListDto<ItemListDto> reqsend = new QueryPagedListDto<ItemListDto>();
            reqsend.PageSize = int.MaxValue;
            reqsend.PageNum = 0;
            reqsend.Search = "";
            reqsend.SortBy = "";
            reqsend.SortDesc = false;
            var result = await _mediator.Send(reqsend);
            
            var list = new List<ItemListDto>();
            list = result.List;

            var properties = new[] { "Id", "Name", "Description", "Code", "Barcode", "UnitMeasureId", "AverageCost", "Cost", "Price", "IsSales", "IsPurchase", "IsStock", "StockMin", "IsArchived", "ImageFileName", "Notes" };
            var headers = new[] { "Id", "Name", "Description", "Code", "Barcode", "UnitMeasureId", "AverageCost", "Cost", "Price", "IsSales", "IsPurchase", "IsStock", "StockMin", "IsArchived", "ImageFileName", "Notes" };

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
                return File(stream.ToArray(), "application/vnd.ms-excel", $"ItemList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xls");
            }
        }

    }
}
