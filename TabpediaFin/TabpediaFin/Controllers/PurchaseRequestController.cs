using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll([FromBody] FetchPagedListRequestDto<PurchaseRequestListDto> request)
        {
            return Result(await _mediator.Send(request));
        }
        
        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTrans(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<PurchaseRequestFetchDto>(id)));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert([FromForm] PurchaseRequestInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromForm] PurchaseRequestUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<PurchaseRequestFetchDto>(id)));
        }
        [HttpPost("CloseTransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Close(int id)
        {
            PurchaseRequestCloseDto command = new PurchaseRequestCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }

    }
}
