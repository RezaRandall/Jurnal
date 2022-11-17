﻿namespace TabpediaFin.Handler;

public class PaymentMethodFetchHandler : IQueryByIdHandler<PaymentMethodDto>
{
    private readonly DbManager _dbManager;
    private readonly ICurrentUser _currentUser;

    public PaymentMethodFetchHandler(DbManager dbManager, ICurrentUser currentUser)
    {
        _dbManager = dbManager;
        _currentUser = currentUser;
    }


    public async Task<RowResponse<PaymentMethodDto>> Handle(QueryByIdDto<PaymentMethodDto> request, CancellationToken cancellationToken)
    {
        var response = new RowResponse<PaymentMethodDto>();

        try
        {
            using (var cn = _dbManager.CreateConnection())
            {
                var row = await cn.FetchAsync<PaymentMethodDto>(request.Id, _currentUser);

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
