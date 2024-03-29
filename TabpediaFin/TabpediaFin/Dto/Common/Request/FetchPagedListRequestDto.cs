﻿namespace TabpediaFin.Dto.Common.Request;

public class FetchPagedListRequestDto<T> : IRequest<PagedListResponse<T>>
{
    public int PageSize { get; set; } = 10;

    public int PageNum { get; set; } = 1;

    public string Search { get; set; } = string.Empty;

    public string SortBy { get; set; } = string.Empty;

    public bool SortDesc { get; set; }
}
