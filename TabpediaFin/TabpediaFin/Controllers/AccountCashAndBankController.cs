

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.CashAndBank;
using TabpediaFin.Handler.ExpenseHandler;

namespace TabpediaFin.Controllers;

[Route("api/account-cash-and-bank")]
[ApiController]
public class AccountCashAndBankController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public AccountCashAndBankController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<AccountCashAndBankListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<AccountCashAndBankFetchDto>(id)));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] AccountCashAndBankInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] AccountCashAndBankUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }


    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteByIdRequestDto<AccountCashAndBankFetchDto>(id));
    }

}
