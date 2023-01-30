using TabpediaFin.Handler.ItemItemCategoryHandler;
using TabpediaFin.Handler.ItemUnitMeasureHandler;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseFetchHandler : IFetchByIdHandler<ExpenseFetchDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public ExpenseFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(FetchByIdRequestDto<ExpenseFetchDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<ExpenseFetchDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("TenantId", _currentUser.TenantId);
                parameters.Add("Id", request.Id);

                var sql = @"SELECT * FROM ""Expense"" WHERE ""TenantId"" = @TenantId AND ""Id"" = @Id ";
                var result = await cn.QueryFirstOrDefaultAsync<ExpenseFetchDto>(sql, parameters);

                if (result != null)
                {
                    // ATTACHMENT
                    var sqlExpenseAttachment = @"SELECT ea.""Id""
                                        , ea.""FileName""
                                        , ea.""FileUrl""
                                        , ea.""Extension""
                                        , ea.""FileSize""
                                        , ea.""TransId"" 
                                        FROM ""ExpenseAttachment"" ea
                                        INNER JOIN ""Expense"" e ON ea.""TransId"" = e.""Id"" 
                                        WHERE e.""TenantId"" = @TenantId AND e.""Id"" = @Id ";
                    List<ExpenseFetchAttachment> resultExpenseAttachment;
                    resultExpenseAttachment = (await cn.QueryAsync<ExpenseFetchAttachment>(sqlExpenseAttachment, parameters).ConfigureAwait(false)).ToList();
                    result.ExpenseAttachmentList = resultExpenseAttachment;

                    // EXPENSE TAG
                    var sqlExpensetag = @"SELECT et.""Id""
                                                ,et.""TagId""
                                                ,et.""TransId""
                                                 FROM ""ExpenseTag"" et
                                                 INNER JOIN ""Expense"" e ON et.""TransId"" = e.""Id"" 
                                                 WHERE e.""TenantId"" = @TenantId AND e.""Id"" = @Id ";

                    //var parametersub = new DynamicParameters();
                    //parametersub.Add("TenantId", _currentUser.TenantId);
                    //parametersub.Add("IdItem", request.Id);

                    List<ExpenseFetchTag> resultExpenseTag;
                    resultExpenseTag = (await cn.QueryAsync<ExpenseFetchTag>(sqlExpensetag, parameters).ConfigureAwait(false)).ToList();
                    result.ExpenseTagList = resultExpenseTag;

                    // EXPENSE LIST
                    var sqlExpenseList = @"SELECT el.""Id"", el.""PriceIncludesTax"", el.""ExpenseAccountId""
                                        , el.""Description"", el.""TaxId"", el.""Amount"", el.""TransId""
                                        FROM ""ExpenseList"" el 
                                        INNER JOIN ""Expense"" e on el.""TransId"" = e.""Id""  
                                        WHERE e.""TenantId"" = @TenantId and e.""Id"" = @Id ";

                    List<ExpenseFetchList> resultExpenseList;
                    resultExpenseList = (await cn.QueryAsync<ExpenseFetchList>(sqlExpenseList, parameters).ConfigureAwait(false)).ToList();
                    result.ExpenseFetchList = resultExpenseList;


                }

                response.IsOk = true;
                response.Row = result;
                response.ErrorMessage = string.Empty;
            }
        }
        catch (Exception ex)
        {
            response.IsOk = false;
            response.Row = null;
            response.ErrorMessage = ex.Message;
        }

        return response;



    }
}

[Table("Expense")]
public class ExpenseFetchDto : BaseDto
{
    public int PayFromAccountId { get; set; } = 0;
    public Boolean PayLater { get; set; } = false;
    public int RecipientContactId { get; set; } = 0;
    public DateTime TransactionDate { get; set; }
    public int PaymentMethodId { get; set; } = 0;
    public string TransactionNo { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public int PaymentTermId { get; set; } = 0;
    public string Memo { get; set; } = string.Empty;
    public int Status { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public Int64 DiscountAmount { get; set; } = 0;
    public int DiscountForAccountId { get; set; } = 0;
    public Int64 TotalAmount { get; set; } = 0;
    public Int64 WitholdingAmount { get; set; } = 0;
    public List<ExpenseFetchAttachment> ExpenseAttachmentList { get; set; }
    public List<ExpenseFetchTag> ExpenseTagList { get; set; }
    public List<ExpenseFetchList> ExpenseFetchList { get; set; }
}


public class ExpenseFetchAttachment : BaseDto
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public int TransId { get; set; }
}

public class ExpenseFetchTag : BaseDto
{
    public int TagId { get; set; }
    public int TransId { get; set; }
}

public class ExpenseFetchList : BaseDto
{
    public bool PriceIncludesTax { get; set; } = false;
    public int ExpenseAccountId { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public Int64 Amount { get; set; } = 0;
    public int TransId { get; set; }
}