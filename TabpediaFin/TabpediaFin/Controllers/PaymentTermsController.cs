using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Dto;
using TabpediaFin.Handler.CashAndBank;
using TabpediaFin.Handler.PaymentTerm;
using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Controllers;

[Route("api/payment-terms")]
[ApiController]
public class PaymentTermController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public PaymentTermController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpPost("list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<PaymentTermListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<PaymentTermDto>(id)));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] PaymentTermInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] PaymentTermUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteByIdRequestDto<PaymentTermDto>(id));
    }

}
