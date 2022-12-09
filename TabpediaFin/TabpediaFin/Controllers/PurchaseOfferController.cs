using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TabpediaFin.Controllers
{
    [Route("api/purchaseoffer")]
    [ApiController]
    public class PurchaseOfferController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        
        public PurchaseOfferController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
        }

        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll([FromBody] FetchPagedListRequestDto<PurchaseOfferListDto> request)
        {
            return Result(await _mediator.Send(request));
        }
        
        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTrans(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<PurchaseOfferFetchDto>(id)));
        }

        //[HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> InsertOfferPurchase([FromBody] PurchaseOfferInsertDto command)
        //{
        //    return Result(await _mediator.Send(command));
        //}

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] PurchaseOfferUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<PurchaseOfferFetchDto>(id)));
        }
        [HttpPost("CloseTransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Close(int id)
        {
            PurchaseOfferCloseDto command = new PurchaseOfferCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
    }
}
