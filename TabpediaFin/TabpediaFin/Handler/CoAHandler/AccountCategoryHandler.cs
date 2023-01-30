namespace TabpediaFin.Handler.CoAHandler
{
    public class AccountCategoryHandler : IListByIdHandler<AccountCategoryDto>
    {
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;
        public AccountCategoryHandler(DbManager dbManager, ICurrentUser currentUser)
        {
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<PagedListResponse<AccountCategoryDto>> Handle(ListByIdRequestDto<AccountCategoryDto> request, CancellationToken cancellationToken)
        {
            var response = new PagedListResponse<AccountCategoryDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    string sqlfilter = "";

                    if (request.Id != 0)
                    {
                        var row = await cn.FetchAsync<AccountCoAFetchDto>(request.Id, _currentUser);

                        if (row.CategoryId == 1)
                        {
                            sqlfilter = @" WHERE ""Id"" = 1 or ""Id"" = 19";
                        }else if (row.IsLocked == true)
                        {
                            sqlfilter = @" WHERE ""Id"" = "+ row.CategoryId +"";
                        }
                    }

                    var sql = @"SELECT  ""Id"", ""Name"" From ""AccountCategory"" " + sqlfilter + " " + @"order by ""Name"" ASC ";

                    var parameters = new DynamicParameters();

                    List<AccountCategoryDto> result;
                    result = (await cn.QueryAsync<AccountCategoryDto>(sql, parameters).ConfigureAwait(false)).ToList();

                    response.IsOk = true;
                    response.List = result;
                    response.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.IsOk = false;
                response.List = null;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }

    [Table("AccountCategory")]
    public class AccountCategoryDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
    }
    public interface IListByIdHandler<T> : IRequestHandler<ListByIdRequestDto<T>, PagedListResponse<T>>
    {
    }

    public class ListByIdRequestDto<T> : FetchByIdRequestDto<int, PagedListResponse<T>>
    {
        public ListByIdRequestDto(int id)
        {
            Id = id;
        }
    }
}
