using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TabpediaFin.Controllers
{
    [Route("api/purchasebilling")]
    [ApiController]
    public class PurchaseBillingController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        
        public PurchaseBillingController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
        }

        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll([FromBody] FetchPagedListRequestDto<PurchaseBillingListDto> request)
        {
            return Result(await _mediator.Send(request));
        }
        
        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTrans(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<PurchaseBillingFetchDto>(id)));
        }

        //[HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> InsertBillingPurchase([FromBody] PurchaseBillingInsertDto command)
        //{
        //    return Result(await _mediator.Send(command));
        //}

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] PurchaseBillingUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<PurchaseBillingFetchDto>(id)));
        }
        [HttpPost("CloseTransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Close(int id)
        {
            PurchaseBillingCloseDto command = new PurchaseBillingCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
    }
}
