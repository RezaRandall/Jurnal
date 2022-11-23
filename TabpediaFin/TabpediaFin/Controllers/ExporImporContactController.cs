using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace TabpediaFin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExporImporContactController : ControllerBase
    {
        [HttpPost("import")]
        public async Task<DemoResponse<List<UserInfo>>> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return DemoResponse<List<UserInfo>>.GetResult(-1, "formfile is empty");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return DemoResponse<List<UserInfo>>.GetResult(-1, "Not Support file extension");
            }

            var list = new List<UserInfo>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new UserInfo
                        {
                            UserName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            Age = int.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()),
                        });
                    }
                }
            }

            // add list to db ..  
            // here just read and return  

            return DemoResponse<List<UserInfo>>.GetResult(0, "OK", list);
        }
    }

    public class UserInfo
    {
        public string UserName { get; set; }

        public int Age { get; set; }
    }

    public class DemoResponse<T>
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }

        public static DemoResponse<T> GetResult(int code, string msg, T data = default(T))
        {
            return new DemoResponse<T>
            {
                Code = code,
                Msg = msg,
                Data = data
            };
        }
    }

}
