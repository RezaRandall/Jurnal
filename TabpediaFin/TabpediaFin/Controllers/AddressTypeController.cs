using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<AddressTypeDto>(id)));
        }
    }
}
