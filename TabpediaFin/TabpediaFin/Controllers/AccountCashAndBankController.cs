using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.CashAndBank;

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
    public async Task<IActionResult> GetList([FromBody] Handler.CashAndBank.QueryPagedListAccountDto<AccountListsDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<AccountFetchDto>(id)));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] AccountInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] AccountUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }


    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteByIdRequestDto<AccountFetchDto>(id));
    }

}
