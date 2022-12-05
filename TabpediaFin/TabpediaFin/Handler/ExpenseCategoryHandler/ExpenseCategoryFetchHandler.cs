namespace TabpediaFin.Handler.ExpenseCategoryHandler
{
    public class ExpenseCategoryFetchHandler : IFetchByIdHandler<ExpenseCategoryFetchDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public ExpenseCategoryFetchHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }
        public async Task<RowResponse<ExpenseCategoryFetchDto>> Handle(FetchByIdRequestDto<ExpenseCategoryFetchDto> request, CancellationToken cancellationToken)
        {
            var response = new RowResponse<ExpenseCategoryFetchDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var row = await cn.FetchAsync<ExpenseCategoryFetchDto>(request.Id, _currentUser);

                    response.IsOk = true;
                    response.Row = row;
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

    [Table("ExpenseCategory")]
    public class ExpenseCategoryFetchDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AccountId { get; set; }
    }
}
