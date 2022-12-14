using Org.BouncyCastle.Asn1.Ocsp;

namespace TabpediaFin.Handler.ExpenseHandler;

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
        if (request.PageNum == 0) { request.PageNum = 1; }
        if (request.PageSize == 0) { request.PageSize = 10; }

        var result = new PagedListResponse<ExpenseListDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                cn.Open();

                var q = await cn.FetchListPagedAsync<ExpenseListDto>(pageNumber: request.PageNum
                , rowsPerPage: request.PageSize
                , search: request.Search
                , sortby: request.SortBy
                    , sortdesc: request.SortDesc
                    , currentUser: _currentUser);

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = q.List;
                result.RecordCount = q.TotalRecord;
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
}