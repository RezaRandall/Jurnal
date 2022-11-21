using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.UnitMeasure;

namespace TabpediaFin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnitMeasuresController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;
    public UnitMeasuresController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    //[HttpGet]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //public async Task<List<UnitMeasureDto>> Get()
    //{
    //    return await _mediator.Send(new UnitMeasureList.Query());
    //}

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new QueryByIdDto<UnitMeasureDto>(id)));
    }

    [HttpGet("list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> getListUnitMeasures(
         [FromQuery] string? searchby
        )
    {
        GetUnitMeasureListQuery param = new GetUnitMeasureListQuery();
        param.searchby = searchby;
        param.TenantId = _currentUser.TenantId;
        var result = await _mediator.Send(param);
        return Ok(result);
    }
    //[HttpGet("getListUnitMeasure")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //public async Task<IActionResult> getListUnitMeasure([FromQuery] string? sortby, [FromQuery] string? valsort, [FromQuery] string? searchby, [FromQuery] string? valsearch, [FromQuery] int? jumlah_data, [FromQuery] int? offset)
    //{
    //    GetUnitMeasureListQuery param = new GetUnitMeasureListQuery();
    //    param.sortby = sortby;
    //    param.valsort = valsort;
    //    param.searchby = searchby;
    //    param.valsearch = valsearch;
    //    param.jumlah_data = jumlah_data;
    //    param.offset = offset;
    //    param.TenantId = _currentUser.TenantId;
    //    var result = await _mediator.Send(param);
    //    return Ok(result);
    //}

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> create([FromBody] AddUnitMeasure unitMeasure)
    {
        unitMeasure.TenantId = _currentUser.TenantId;
        unitMeasure.CreatedUid = _currentUser.UserId;
        var result = await _mediator.Send(unitMeasure);
        return Ok(result);
    }

    [HttpPut("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Put([FromBody] UpdateUnitMeasure unitMeasure)
    {
        unitMeasure.TenantId = _currentUser.TenantId;
        unitMeasure.UpdatedUid = _currentUser.UserId;
        var result = await _mediator.Send(unitMeasure);
        return Ok(result);
    }

    [HttpDelete("delete/{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {
        UnitMeasureRespons response = new UnitMeasureRespons();
        DeleteUnitMeasure param = new DeleteUnitMeasure();
        param.Id = id;
        param.TenantId = _currentUser.TenantId;
        var result = await _mediator.Send(param);
        if (result == true)
        {
            response.status = "success";
            response.message = "unit measure with id " + id + " was deleted";
        }
        else
        {
            response.status = "failed";
            response.message = "Data not found";
        }
        return Ok(response);
    }


}
