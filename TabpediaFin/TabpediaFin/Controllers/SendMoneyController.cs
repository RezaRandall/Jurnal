using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.CashAndBank;
using TabpediaFin.Handler.SendMoneyHandler;

namespace TabpediaFin.Controllers;

[Route("api/send-money")]
[ApiController]
public class SendMoneyController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public SendMoneyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<SendMoneyListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<SendMoneyFetchDto>(id)));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] SendMoneyInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] SendMoneyUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteByIdRequestDto<SendMoneyFetchDto>(id));
    }


}
