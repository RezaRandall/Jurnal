using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TabpediaFin.Controllers
{
    [Route("api/PurchaseOrder")]
    [ApiController]
    public class PurchaseOrderController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        
        public PurchaseOrderController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
        }

        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll([FromBody] FetchPagedListRequestDto<PurchaseOrderListDto> request)
        {
            return Result(await _mediator.Send(request));
        }
        
        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTrans(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<PurchaseOrderFetchDto>(id)));
        }

        //[HttpPost("PurchaseOrder")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> InsertOrderPurchase([FromBody] PurchaseOrderInsertDto command)
        //{
        //    return Result(await _mediator.Send(command));
        //}

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] PurchaseOrderUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<PurchaseOrderFetchDto>(id)));
        }
        [HttpPost("CloseTransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Close(int id)
        {
            PurchaseOrderCloseDto command = new PurchaseOrderCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
    }
}
