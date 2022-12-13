using TabpediaFin.Helper;

namespace TabpediaFin.Handler.UploadAttachmentHandler
{
    public class UploadAttachmentHandler : IRequestHandler<requestupload, RowResponse<uploadreturn>>
    {
        private readonly ICurrentUser _currentUser;
        public UploadAttachmentHandler(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        public async Task<RowResponse<uploadreturn>> Handle(requestupload request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<uploadreturn>();
            try
            {
                var uniqueFileName = FileUploadHelper.GetUniqueFileName(request.fileupload.FileName);
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "UserUpload", "posts", _currentUser.TenantId.ToString());
                string extension = System.IO.Path.GetExtension(request.fileupload.FileName);
                var filePath = Path.Combine(uploads, uniqueFileName);
                string host = "https://finapidev.tabpedia.com/";
                var FileUrl = host + "UserUpload/posts/" + _currentUser.TenantId.ToString() + "/" + uniqueFileName;
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                await request.fileupload.CopyToAsync(new FileStream(filePath, FileMode.Create));

                uploadreturn listreturn = (new uploadreturn
                {
                    FileName = uniqueFileName,
                    FileUrl = FileUrl,
                    FileSize = request.fileupload.Length.ToString(),
                    Extension = extension,
                });


                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = listreturn;
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }

    public class uploadreturn
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
    }

    public class requestupload : IRequest<RowResponse<uploadreturn>>
    {
        public IFormFile fileupload;
    }
}
