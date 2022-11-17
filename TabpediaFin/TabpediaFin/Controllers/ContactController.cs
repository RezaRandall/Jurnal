using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TabpediaFin.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("group/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetGroup(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactGroupDto>(id)));
        }

        [HttpGet("address/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAddress(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactAddressDto>(id)));
        }

        [HttpGet("person/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPerson(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactPersonDto>(id)));
        }
    }
}
