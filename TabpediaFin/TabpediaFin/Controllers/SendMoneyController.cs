using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.SendMoneyHandler;

namespace TabpediaFin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SendMoneyController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public SendMoneyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/SendMoney/list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<SendMoneyListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("/SendMoney{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<SendMoneyFetchDto>(id)));
    }

    [HttpPost("/SendMoney/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromForm] SendMoneyInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut("/SendMoney/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] SendMoneyUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {
        SendMoneyDeleteDto command = new SendMoneyDeleteDto();
        command.Id = id;
        return Result(await _mediator.Send(command));
    }


}
