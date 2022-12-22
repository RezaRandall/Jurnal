﻿namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseListHandler : IFetchPagedListHandler<ExpenseListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public ExpenseListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<ExpenseListDto>> Handle(FetchPagedListRequestDto<ExpenseListDto> request, CancellationToken cancellationToken)
    {
        if (request.PageNum == 0) { request.PageNum = 0; }
        if (request.PageSize == 0) { request.PageSize = 10; }

        var response = new PagedListResponse<ExpenseListDto>();

        try
        {
            using (var conn = _dbManager.CreateConnection())
            {
                string sqlsort = "";
                string sqlsearch = "";
                string expensefilter = "";

                if (request.SortBy != null && request.SortBy != "")
                {
                    sqlsort = @" order by """ + request.SortBy + "\" ASC";

                    if (request.SortDesc == true)
                    {
                        sqlsort = @" order by """ + request.SortBy + "\" ASC";
                    }
                }

                if (request.Search != null && request.Search != "")
                {
                    sqlsearch = @"AND LOWER(""TransNum"") LIKE @Search OR LOWER(""Notes"") LIKE @Search OR LOWER(""Description"") LIKE @Search";
                }

                var sql = @"SELECT  *
                          FROM ""Expense"" WHERE ""TenantId"" = @TenantId " + expensefilter + " " + sqlsearch + " " + sqlsort + " LIMIT @PageSize OFFSET @PageNum";

                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("PageSize", request.PageSize);
                parameters.Add("PageNum", request.PageNum);
                parameters.Add("Search", $"%{request.Search.Trim().ToLowerInvariant()}%");

                List<ExpenseListDto> result;
                result = (await conn.QueryAsync<ExpenseListDto>(sql, parameters).ConfigureAwait(false)).ToList();

                response.RecordCount = result.Count;

                response.IsOk = true;
                response.List = result;
                response.ErrorMessage = string.Empty;
            }
        }
        catch (Exception ex)
        {
            response.IsOk = false;
            response.ErrorMessage = ex.Message;
        }
        return response;
    }
}

[Table("Expense")]
public class ExpenseListDto : BaseDto
{
    [Searchable]
    public string TransNum { get; set; } = string.Empty;
    public DateTime? TransDate { get; set; }
    public int ContactId { get; set; } = 0;
    public int PaymentMethodId { get; set; } = 0;
    public int PaymentTermId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public int DiscountTypeId { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    [Searchable]
    public string Notes { get; set; } = string.Empty;
    [Searchable]
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int AccountCashAndBankId { get; set; } = 0;
    public Boolean PayLater { get; set; } = false;
    public Boolean PriceIncludesTax { get; set; } = false;
}