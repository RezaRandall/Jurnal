using TabpediaFin.Handler.Item;
using TabpediaFin.Handler.UnitMeasure;

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
    public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<ItemListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("/item{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new QueryByIdDto<ItemDto>(id)));
    }

    [HttpPost("/item/create")]
    public async Task<IActionResult> Insert([FromBody] ItemInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }


    [HttpPut("/item/update")]
    public async Task<IActionResult> Update([FromBody] ItemUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    //[HttpDelete("/item/delete/{id}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    //return Result(await _mediator.Send(command));
    //    //UnitMeasureRespons response = new UnitMeasureRespons();
    //    var response = new RowResponse<ItemDto>();
    //    ItemDeleteDto param = new ItemDeleteDto();
    //    param.Id = id;
    //    //param.TenantId = _currentUser.TenantId;
    //    var result = await _mediator.Send(param);
    //    if (result == true)
    //    {
    //        response.status = "success";
    //        response.message = "unit measure with id " + id + " was deleted";
    //    }
    //    else
    //    {
    //        response.status = "failed";
    //        response.message = "Data not found";
    //    }
    //    return Ok(response);
    //}

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _mediator.Send(new ItemDeleteHandler.Command(id));
    }


}
