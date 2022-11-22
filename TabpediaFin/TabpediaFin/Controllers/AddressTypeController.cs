using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.AddressTypeHandler;

namespace TabpediaFin.Controllers
{
    [Route("api/addresstype")]
    [ApiController]
    public class AddressTypeController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AddressTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<AddressTypeListDto> request)
        {
            return Result(await _mediator.Send(request));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<AddressTypeFetchDto>(id)));
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert([FromBody] AddressTypeInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] AddressTypeUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {

            AddressTypeDeleteDto command = new AddressTypeDeleteDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
    }
}
