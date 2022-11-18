namespace TabpediaFin.Handler.Product;

public class PaymentTermList
{
    public class Query : IRequest<List<PaymentTermDto>>
    {
        //public string SearchBy { get; }

        public Query
           (
           //string searchBy = ""
           )
        {
            //SearchBy = searchBy;;
        }
    }

    public class QueryHandler : IRequestHandler<Query, List<PaymentTermDto>>
    {
        private readonly IDbConnection _dbConnection;
        private readonly ICurrentUser _currentUser;

        public QueryHandler(
            IDbConnection connection,
            ICurrentUser currentUser)
        {
            _dbConnection = connection;
            _currentUser = currentUser;
        }

        //Implement Paging
        public async Task<List<PaymentTermDto>> Handle(
            Query message,
            CancellationToken cancellationToken)
        {
            //var searchByCriteria = message.SearchBy
            //    ? $" AND STRFTIME('%Y', e.Date) = '{message.Year}'"
            //    : string.Empty;
            var sql = $@"SELECT * FROM ""PaymentTerm"" WHERE ""TenantId"" = @TenantId ";

            var paymentTerms = await _dbConnection.QueryAsync<PaymentTermDto>(sql, new
            {
                tenantId = _currentUser.TenantId,
            });

            return paymentTerms.ToList();
        }
    }

}


