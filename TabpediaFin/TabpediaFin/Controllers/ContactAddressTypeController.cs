using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.ContactAddressTypeHandler;

namespace TabpediaFin.Controllers
{
    [Route("api/addresstype")]
    [ApiController]
    public class ContactAddressTypeController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ContactAddressTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<ContactAddressTypeListDto> request)
        {
            return Result(await _mediator.Send(request));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactAddressTypeFetchDto>(id)));
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert([FromBody] ContactAddressTypeInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] ContactAddressTypeUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {

            ContactAddressTypeDeleteDto command = new ContactAddressTypeDeleteDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
    }
}
