//using TabpediaFin.Helper;
//namespace TabpediaFin.Handler.UploadAttachmentHandler
//{
//    public class UploadAttachmentService : IRequestHandler<requestupload, RowResponse<uploadreturn>>
//    {
//        private readonly ICurrentUser _currentUser;
//        public UploadAttachmentService(ICurrentUser currentUser)
//        {
//            _currentUser = currentUser;
//        }

//        public async Task<RowResponse<uploadreturn>> Handle(requestupload request, CancellationToken cancellationToken)
//        {
//            var result = new RowResponse<uploadreturn>();
//            try
//            {
//                var uniqueFileName = FileUploadHelper.GetUniqueFileName(request.fileupload.FileName);
//                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "UserUpload", "posts", _currentUser.TenantId.ToString());
//                string extension = System.IO.Path.GetExtension(request.fileupload.FileName);
//                var filePath = Path.Combine(uploads, uniqueFileName);
//                string host = "https://localhost:7030/";
//                var FileUrl = host + "UserUpload/posts/" + _currentUser.TenantId.ToString() + "/" + uniqueFileName;
//                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

//                await request.fileupload.CopyToAsync(new FileStream(filePath, FileMode.Create));

//                uploadreturn listreturn = (new uploadreturn
//                {
//                    FileName = uniqueFileName,
//                    FileUrl = FileUrl,
//                    FileSize = request.fileupload.Length.ToString(),
//                    Extension = extension,
//                });


//                result.IsOk = true;
//                result.ErrorMessage = string.Empty;
//                result.Row = listreturn;
//            }
//            catch (Exception ex)
//            {
//                result.IsOk = false;
//                result.ErrorMessage = ex.Message;
//            }

//            return result;
//        }

//        //    public async Task<List<uploadreturn>> UploadAttachmentBase64Async(ICollection<string> param, int TenantId, int TransId)
//        //    {
//        //        List<uploadreturn> listreturn = new List<uploadreturn>();
//        //        foreach (string item in param)
//        //        {
//        //            byte[] bytes = Convert.FromBase64String(item);
//        //            var uniqueFileName = FileUploadHelper.GetUniqueFileName(DateTime.Now.ToString());
//        //            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "UserUpload", "posts", TenantId.ToString());
//        //            var filePath = Path.Combine(uploads, uniqueFileName);
//        //            File.WriteAllBytes(uploads, bytes);
//        //        }
//        //        foreach (IFormFile item in param)
//        //        {
//        //            var uniqueFileName = FileUploadHelper.GetUniqueFileName(item.FileName);
//        //            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "UserUpload", "posts", TenantId.ToString());
//        //            string extension = System.IO.Path.GetExtension(item.FileName);
//        //            var filePath = Path.Combine(uploads, uniqueFileName);
//        //            string host = "https://localhost:7030/";
//        //            var FileUrl = host + "UserUpload/posts/" + TenantId.ToString() + "/" + uniqueFileName;
//        //            Directory.CreateDirectory(Path.GetDirectoryName(filePath));


//        //            await item.CopyToAsync(new FileStream(filePath, FileMode.Create));

//        //            listreturn.Add(new uploadreturn
//        //            {
//        //                FileName = uniqueFileName,
//        //                FileUrl = FileUrl,
//        //                FileSize = item.Length.ToString(),
//        //                Extension = extension,
//        //                TransId = TransId
//        //            });
//        //        }

//        //        return listreturn;
//        //    }
//    }

//    //public class uploadreturn
//    //{
//    //    public string FileName { get; set; } = string.Empty;
//    //    public string FileUrl { get; set; } = string.Empty;
//    //    public string FileSize { get; set; } = string.Empty;
//    //    public string Extension { get; set; } = string.Empty;
//    //}

//    //public class requestupload{
//    //    public IFormFile fileupload;
//    //}

//}
