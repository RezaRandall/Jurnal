namespace TabpediaFin.Dto.Common.Request
{
    public class QueryReadFileExcelDto<T> : IRequest<PagedListResponse<T>>
    {
        public IFormFile formFile { get; set; }
    }
}
