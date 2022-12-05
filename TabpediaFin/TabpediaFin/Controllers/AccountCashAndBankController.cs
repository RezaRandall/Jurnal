

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.CashAndBank;
using TabpediaFin.Handler.ExpenseHandler;

namespace TabpediaFin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountCashAndBankController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public AccountCashAndBankController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/AccountCashAndBank/list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<AccountCashAndBankListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("/AccountCashAndBank{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new QueryByIdDto<AccountCashAndBankFetchDto>(id)));
    }

    [HttpPost("/AccountCashAndBank/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] AccountCashAndBankInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut("/AccountCashAndBank/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] AccountCashAndBankUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {

        AccountCashAndBankDeleteDto command = new AccountCashAndBankDeleteDto();
        command.Id = id;
        return Result(await _mediator.Send(command));
    }

}
