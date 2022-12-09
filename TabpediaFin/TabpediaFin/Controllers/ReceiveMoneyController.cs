using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.ReceiveMoneyHandler;

namespace TabpediaFin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReceiveMoneyController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ReceiveMoneyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/ReceiveMoney/list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<ReceiveMoneyListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("/ReceiveMoney{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<ReceiveMoneyFetchDto>(id)));
    }

    [HttpPost("/ReceiveMoney/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromForm] ReceiveMoneyInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut("/ReceiveMoney/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] ReceiveMoneyUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {
        ReceiveMoneyDeleteDto command = new ReceiveMoneyDeleteDto();
        command.Id = id;
        return Result(await _mediator.Send(command));
    }
}
