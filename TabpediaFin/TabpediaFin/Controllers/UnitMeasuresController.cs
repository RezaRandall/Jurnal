using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static TabpediaFin.Dto.UnitMeasureDto;

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

    [HttpGet("getListUnitMeasures")]
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

    //[HttpGet("getUnitMeasure/{id:int}")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //public async Task<IActionResult> GetUnitMeasureById(int id)
    //{
    //    GetUnitMeasureQuery param = new GetUnitMeasureQuery();
    //    param.Id = id;
    //    param.TenantId = _currentUser.TenantId;
    //    var result = await _mediator.Send(param);
    //    return Ok(result);
    //}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new QueryByIdDto<UnitMeasureDto>(id)));
    }

    [HttpPost("createUnitMeasure")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> create([FromBody] AddUnitMeasure unitMeasure)
    {
        unitMeasure.TenantId = _currentUser.TenantId;
        unitMeasure.CreatedUid = _currentUser.UserId;
        var result = await _mediator.Send(unitMeasure);
        return Ok(result);
    }

    [HttpPut("updateUnitMeasure")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Put([FromBody] UpdateUnitMeasure unitMeasure)
    {
        unitMeasure.TenantId = _currentUser.TenantId;
        unitMeasure.UpdatedUid = _currentUser.UserId;
        var result = await _mediator.Send(unitMeasure);
        return Ok(result);
    }

    [HttpDelete("deleteUnitMeasure/{id:int}")]
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
