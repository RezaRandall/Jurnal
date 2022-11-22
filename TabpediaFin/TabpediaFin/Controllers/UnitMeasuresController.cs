using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using TabpediaFin.Handler.UnitMeasures;

namespace TabpediaFin.Controllers;

[Route("api/unit-measure")]
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

    [HttpPost("/unit-measure/list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<UnitMeasureListDto> request)
    {
        return Result(await _mediator.Send(request));
    }

    [HttpGet("/unit-measure{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new QueryByIdDto<UnitMeasureDto>(id)));
    }

    [HttpPost("/unit-measure/create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Insert([FromBody] UnitMeasureInsertDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpPut("/unit-measure/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update([FromBody] UnitMeasureUpdateDto command)
    {
        return Result(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {

        UnitMeasureDeleteDto command = new UnitMeasureDeleteDto();
        command.Id = id;
        return Result(await _mediator.Send(command));
    }




}
