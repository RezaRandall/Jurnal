using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.ContactGroupHandler;

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

        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<ContactGroupListDto> request)
        {
            return Result(await _mediator.Send(request));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactGroupFetchDto>(id)));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert([FromBody] ContactGroupInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ContactGroupUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
    }
}
