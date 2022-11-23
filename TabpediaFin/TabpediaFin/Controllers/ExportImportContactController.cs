using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using TabpediaFin.Handler.ExportImportContactHandler;

namespace TabpediaFin.Controllers
{
    [Route("api/exporimporcontact")]
    [ApiController]
    public class ExportImportContactController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ExportImportContactController(IMediator mediator)
        {
            _mediator = mediator;
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
        [HttpGet("export")]
        public async Task<IActionResult> ExportV2(CancellationToken cancellationToken)
        {
            // query data from database  
            await Task.Yield();
            var list = new List<UserInfo>()
            {
                new UserInfo { UserName = "catcher", Age = 18 },
                new UserInfo { UserName = "james", Age = 20 },
            };
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(list, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"ContactList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public class UserInfo
        {
            public string UserName { get; set; }

            public int Age { get; set; }
        }

    }
}
