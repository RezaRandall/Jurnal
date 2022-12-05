using TabpediaFin.Helper;
namespace TabpediaFin.Handler.UploadAttachmentHandler
{
    public class UploadAttachmentService
    {

        private readonly IWebHostEnvironment environment;

        public UploadAttachmentService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task<List<uploadreturn>> UploadAttachmentAsync(ICollection<IFormFile> param, int TenantId, int TransId)
        {
            List<uploadreturn> listreturn = new List<uploadreturn>();
            foreach(IFormFile item in param)
            {
                var uniqueFileName = FileUploadHelper.GetUniqueFileName(item.FileName);
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "UserUpload", "posts", TenantId.ToString());
                string extension = System.IO.Path.GetExtension(item.FileName);
                var filePath = Path.Combine(uploads, uniqueFileName);
                string host = "https://localhost:7030/";
                var FileUrl = host + "UserUpload/posts/"+ TenantId.ToString() + "/"+ uniqueFileName;
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));


                await item.CopyToAsync(new FileStream(filePath, FileMode.Create));

                listreturn.Add(new uploadreturn
                {
                    FileName = uniqueFileName,
                    FileUrl = FileUrl,
                    FileSize = item.Length,
                    Extension = extension,
                    TransId = TransId
                });
            }

            return listreturn;
        }
    }

    public class uploadreturn
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Extension { get; set; } = string.Empty;
        public int TransId { get; set; }
    }
}
