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
    public string TransNum { get; set; } = string.Empty;
    public DateTime? TransDate { get; set; }
    public int ContactId { get; set; } = 0;
    public int PaymentMethodId { get; set; } = 0;
    public int PaymentTermId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public int DiscountTypeId { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public string Notes { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public List<ExpenseFetchTag> ExpenseTagList { get; set; }
    public List<ExpenseFetchAttachment> ExpenseAttachmentList { get; set; }
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