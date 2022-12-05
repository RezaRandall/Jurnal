
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.ExpenseHandler;

namespace TabpediaFin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/Expense/list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<ExpenseListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("/Expense{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<ExpenseFetchDto>(id)));
    }

    [HttpPost("/Expense/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] ExpenseInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut("/Expense/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] ExpenseUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {

        ExpenseDeleteDto command = new ExpenseDeleteDto();
        command.Id = id;
        return Result(await _mediator.Send(command));
    }
}
