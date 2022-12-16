namespace TabpediaFin.Handler.SendMoneyHandler;

public class SendMoneyListHandler : IFetchPagedListHandler<SendMoneyListDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public SendMoneyListHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<PagedListResponse<SendMoneyListDto>> Handle(FetchPagedListRequestDto<SendMoneyListDto> req, CancellationToken cancellationToken)
    {
        if (req.PageNum == 0) { req.PageNum = 1; }
        if (req.PageSize == 0) { req.PageSize = 10; }

        var result = new PagedListResponse<SendMoneyListDto>();

        try
        {
            string sqlWhere = " WHERE (1=1) ";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                sqlWhere += SqlHelper.GenerateWhere<SendMoneyListDto>();
                parameters.Add("Search", $"%{req.Search.Trim().ToLowerInvariant()}%");
            }

            //var orderby = string.Empty;
            if (string.IsNullOrWhiteSpace(req.SortBy))
            {
                req.SortBy = SqlHelper.GenerateOrderBy(req.SortBy, req.SortDesc, "CreatedUtc");
            }

            using (var cn = _dbManager.CreateConnection())
            {
                cn.Open();

                var list = await cn.FetchListPagedAsync<SendMoneyListDto>(pageNumber: req.PageNum
                   , rowsPerPage: req.PageSize
                   , search: req.Search
                   , sortby: req.SortBy
                   , sortdesc: req.SortDesc
                   , currentUser: _currentUser
                    );
                int recordCount = await cn.RecordCountAsync<SendMoneyListDto>(sqlWhere, parameters);
                result.IsOk = true;
                result.ErrorMessage = string.Empty;
                result.List = list.List;
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

[Table("SendMoney")]
public class SendMoneyListDto : BaseDto
{
    public int PayFromAccountId { get; set; } = 0;
    public int ReceiverVendorId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public string TransactionNo { get; set; } = string.Empty;
    public bool PriceIncludesTax { get; set; } = false;
    public int AccountCashAndBankId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int TotalAmount { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
}