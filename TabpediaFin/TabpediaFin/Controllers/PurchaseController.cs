using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TabpediaFin.Controllers
{
    [Route("api/purchase")]
    [ApiController]
    public class PurchaseController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
        }

        [HttpPost("request/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllRequest([FromBody] FetchPagedListRequestDto<PurchaseRequestListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("request/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTransRequest(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<PurchaseRequestFetchDto>(id)));
        }

        [HttpPost("request")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertRequest([FromBody] PurchaseRequestInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("request")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateRequest([FromBody] PurchaseRequestUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("request/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<PurchaseRequestFetchDto>(id)));
        }
        [HttpPost("request/closetransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CloseRequest(int id)
        {
            PurchaseRequestCloseDto command = new PurchaseRequestCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
        [HttpPost("offer/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllOffer([FromBody] FetchPagedListRequestDto<PurchaseOfferListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("offer/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTransOffer(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<PurchaseOfferFetchDto>(id)));
        }

        [HttpPost("offer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertOffer([FromBody] PurchaseOfferInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("offer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateOffer([FromBody] PurchaseOfferUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("offer/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<PurchaseOfferFetchDto>(id)));
        }
        [HttpPost("offer/closetransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CloseOffer(int id)
        {
            PurchaseOfferCloseDto command = new PurchaseOfferCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }

        [HttpPost("order/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllOrder([FromBody] FetchPagedListRequestDto<PurchaseOrderListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("order/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTransOrder(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<PurchaseOrderFetchDto>(id)));
        }

        [HttpPost("order")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertOrder([FromBody] PurchaseOrderInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("order")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateOrder([FromBody] PurchaseOrderUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("order/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<PurchaseOrderFetchDto>(id)));
        }
        [HttpPost("order/closetransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CloseOrder(int id)
        {
            PurchaseOrderCloseDto command = new PurchaseOrderCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
        [HttpPost("billing/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllBilling([FromBody] FetchPagedListRequestDto<PurchaseBillingListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("billing/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTransBilling(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<PurchaseBillingFetchDto>(id)));
        }

        [HttpPost("billing")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertBillingPurchase([FromBody] PurchaseBillingInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("billing")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateBilling([FromBody] PurchaseBillingUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("billing/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteBilling(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<PurchaseBillingFetchDto>(id)));
        }
        [HttpPost("billing/closetransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CloseBilling(int id)
        {
            PurchaseBillingCloseDto command = new PurchaseBillingCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
    }
}
