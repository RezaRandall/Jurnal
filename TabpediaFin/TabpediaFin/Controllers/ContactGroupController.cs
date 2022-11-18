using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TabpediaFin.Controllers
{
    [Route("api/contactgroup")]
    [ApiController]
    public class ContactGroupController :ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ContactGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetGroup(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactGroupDto>(id)));
        }
    }
}
