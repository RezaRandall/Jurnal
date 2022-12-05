using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.ContactHandler;
using TabpediaFin.Handler.Item;

namespace TabpediaFin.Controllers;

[Route("api/item")]
public class ItemController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public ItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/item/list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] FetchPagedListRequestDto<ItemListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("/item{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new FetchByIdRequestDto<ItemDto>(id)));
    }

    [HttpPost("/item/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] ItemInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut("/item/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] ItemUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {

        ItemDeleteDto command = new ItemDeleteDto();
        command.Id = id;
        return Result(await _mediator.Send(command));
    }
}
