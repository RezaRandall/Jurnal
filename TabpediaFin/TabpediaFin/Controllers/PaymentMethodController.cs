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
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<PaymentMethodListDto> request)
    {
        return Result(await _mediator.Send(request));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<PaymentMethodDto>(id)));
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return Result(await _mediator.Send(new CreateRequestDto<PaymentMethodDto>()));
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


    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteByIdRequestDto<PaymentMethodDto>(id));
    }
}
