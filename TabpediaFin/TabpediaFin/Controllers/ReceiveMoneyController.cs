using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.ExpenseHandler;
using TabpediaFin.Handler.ReceiveMoneyHandler;
using TabpediaFin.Handler.TransferMoneyHandler;

namespace TabpediaFin.Controllers;

[Route("api/receive-money")]
[ApiController]
public class ReceiveMoneyController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ReceiveMoneyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<ReceiveMoneyListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<ReceiveMoneyFetchDto>(id)));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] ReceiveMoneyInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] ReceiveMoneyUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteByIdRequestDto<ReceiveMoneyFetchDto>(id));
    }
}
