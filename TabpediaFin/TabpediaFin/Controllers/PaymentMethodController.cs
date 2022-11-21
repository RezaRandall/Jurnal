namespace TabpediaFin.Controllers;

[Route("api/payment-method")]
public class PaymentMethodController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PaymentMethodController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<PaymentMethodListDto> request)
    {
        return Result(await _mediator.Send(request));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new QueryByIdDto<PaymentMethodDto>(id)));
    }


    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] PaymentMethodInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }


    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PaymentMethodUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

}
