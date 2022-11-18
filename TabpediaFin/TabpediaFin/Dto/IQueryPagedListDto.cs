namespace TabpediaFin.Dto
{
    public interface IQueryPagedListDto
    {
        int PageNum { get; set; }
        int PageSize { get; set; }
        string Search { get; set; }
        string SortBy { get; set; }
        bool SortDesc { get; set; }
    }
}