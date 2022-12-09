using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.TransferMoneyHandler;

namespace TabpediaFin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransferMoneyController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public TransferMoneyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/TransferMoney/list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<TransferMoneyListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("/TransferMoney{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<TransferMoneyFetchDto>(id)));
    }

    [HttpPost("/TransferMoney/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromForm] TransferMoneyInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut("/TransferMoney/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] TransferMoneyUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {
        TransferMoneyDeleteDto command = new TransferMoneyDeleteDto();
        command.Id = id;
        return Result(await _mediator.Send(command));
    }


}
