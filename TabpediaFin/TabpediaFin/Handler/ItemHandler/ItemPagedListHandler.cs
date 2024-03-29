﻿namespace TabpediaFin.Handler.Item;

public class ItemPagedListHandler : IQueryPagedListItemHandler<ItemListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public ItemPagedListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<ItemListDto>> Handle(QueryPagedListItemDto<ItemListDto> req, CancellationToken cancellationToken)
    {
        if (req.PageNum == 0) { req.PageNum = 1; }
        if (req.PageSize == 0) { req.PageSize = 10; }

        var result = new PagedListResponse<ItemListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            if(req.isArchived == false)
            {
                sqlWhere += @"And ""IsArchived"" = false";
            }
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<ItemListDto>();
                parameters.Add("Search", $"%{req.Search.Trim().ToLowerInvariant()}%");
            }

            var orderby = string.Empty;
            if (string.IsNullOrWhiteSpace(req.SortBy))
            {
                orderby = SqlHelper.GenerateOrderBy(req.SortBy, req.SortDesc);
            }

            using (var cn = _dbManager.CreateConnection())
            {
                cn.Open();

                var list = await cn.FetchListPagedAsync<ItemListDto>(pageNumber: req.PageNum
                , rowsPerPage: req.PageSize
                , conditions: sqlWhere
                , orderby: orderby
                , currentUser: _currentUser
                , parameters: parameters);

                int recordCount = await cn.RecordCountAsync<ItemListDto>(sqlWhere, parameters);

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list?.AsList() ?? new List<ItemListDto>(); ;
                result.RecordCount = recordCount;
            }
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }


}

[Table("Item")]
public class ItemListDto : BaseDto
{
    [Searchable]
    public string Name { get; set; } = string.Empty;
    [Searchable]
    public string Description { get; set; } = string.Empty;
    [Searchable]
    public string Code { get; set; } = string.Empty;
    [Searchable]
    public string Barcode { get; set; } = string.Empty;
    public int UnitMeasureId { get; set; } = 0;
    public int AverageCost { get; set; } = 0;
    public int Cost { get; set; } = 0;
    public int Price { get; set; } = 0;
    public bool IsSales { get; set; } = true;
    public bool IsPurchase { get; set; } = true;
    public bool IsStock { get; set; } = true;
    public int StockMin { get; set; } = 0;
    public bool IsArchived { get; set; } = true;
    [Searchable]
    public string ImageFileName { get; set; } = string.Empty;
    [Searchable]
    public string Notes { get; set; } = string.Empty;
}
public class QueryPagedListItemDto<T> : IRequest<PagedListResponse<T>>
{
    public int PageSize { get; set; } = 10;

    public int PageNum { get; set; } = 1;

    public string Search { get; set; } = string.Empty;

    public string SortBy { get; set; } = string.Empty;

    public bool SortDesc { get; set; }
    public bool isArchived { get; set; } = false;
}
public interface IQueryPagedListItemHandler<T> : IRequestHandler<QueryPagedListItemDto<T>, PagedListResponse<T>>
{
}
