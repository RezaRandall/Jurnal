using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.WarehouseHandler;

namespace TabpediaFin.Controllers
{
    [Route("api/warehouse")]
    [ApiController]
    public class WarehouseController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public WarehouseController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<WarehouseListDto> request)
        {
            return Result(await _mediator.Send(request));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<WarehouseFetchDto>(id)));
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert([FromBody] WarehouseInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] WarehouseUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {

            WarehouseDeleteDto command = new WarehouseDeleteDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
    }
}
