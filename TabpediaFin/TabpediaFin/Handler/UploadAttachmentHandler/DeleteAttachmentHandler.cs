namespace TabpediaFin.Handler.UploadAttachmentHandler
{
    public class DeleteAttachmentHandler : IRequestHandler<deletefile, RowResponse<uploadreturn>>
    {

        public DeleteAttachmentHandler()
        {
        }

        public async Task<RowResponse<uploadreturn>> Handle(deletefile request, CancellationToken cancellationToken)
        {
            var result = new RowResponse<uploadreturn>();

            try
            {
                FileInfo file = new FileInfo(request.FileUrl.Replace("https://localhost:7030/", "../TabpediaFin/"));
                if (file.Exists)
                {
                    file.Delete();
                }


                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.Row = new uploadreturn();
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
    public class deletefile : IRequest<RowResponse<uploadreturn>>
    {
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
    }
}
