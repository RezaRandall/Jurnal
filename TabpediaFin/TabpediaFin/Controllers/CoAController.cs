using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TabpediaFin.Controllers
{
    [Route("api/coa")]
    [ApiController]
    public class CoAController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CoAController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("generateaccountfirst")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GenerateAccountFirst()
        {
            GenerateAccountInsertDto command = new GenerateAccountInsertDto();
            return Result(await _mediator.Send(command));
        }
    }
}
