namespace TabpediaFin.Handler.PaymentMethodHandler;

public class PaymentMethodCreateHandler : ICreateHandler<PaymentMethodDto>
{
    public Task<RowResponse<PaymentMethodDto>> Handle(CreateRequestDto<PaymentMethodDto> request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<PaymentMethodDto>()
        {
            IsOk = true,
            ErrorMessage = string.Empty,
            Row = new PaymentMethodDto()
        };

        return Task.FromResult(result);
    }
}
