using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.PurchaseRequestHandler;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TabpediaFin.Controllers
{
    [Route("api/PurchaseRequest")]
    [ApiController]
    public class PurchaseRequestController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        
        public PurchaseRequestController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert([FromForm] PurchaseRequestInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }
       
    }
}
