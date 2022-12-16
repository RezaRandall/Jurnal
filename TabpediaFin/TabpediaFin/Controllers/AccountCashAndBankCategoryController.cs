using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.CashAndBankCategoryHandler;

namespace TabpediaFin.Controllers;

[Route("api/account-cash-and-bank-category")]
[ApiController]
public class AccountCashAndBankCategoryController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public AccountCashAndBankCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<AccountCashAndBankCategoryListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<AccountCashAndBankCategoryFetchDto>(id)));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] AccountCashAndBankCategoryInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] AccountCashAndBankCategoryUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }


    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _mediator.Send(new DeleteByIdRequestDto<AccountCashAndBankCategoryFetchDto>(id));
    }



}
