namespace TabpediaFin.Controllers;

[Route("payment-method")]
public class PaymentMethodController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PaymentMethodController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new QueryByIdDto<PaymentMethodDto>(id)));
    }

}
