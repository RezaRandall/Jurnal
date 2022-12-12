using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TabpediaFin.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
        }
        [HttpPost("offer/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllOffer([FromBody] FetchPagedListRequestDto<SalesOfferListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("offer/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTransOffer(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<SalesOfferFetchDto>(id)));
        }

        [HttpPost("offer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertOffer([FromBody] SalesOfferInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("offer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateOffer([FromBody] SalesOfferUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("offer/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<SalesOfferFetchDto>(id)));
        }
        [HttpPost("offer/closetransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CloseOffer(int id)
        {
            SalesOfferCloseDto command = new SalesOfferCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }

        [HttpPost("order/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllOrder([FromBody] FetchPagedListRequestDto<SalesOrderListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("order/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTransOrder(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<SalesOrderFetchDto>(id)));
        }

        [HttpPost("order")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertOrder([FromBody] SalesOrderInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("order")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateOrder([FromBody] SalesOrderUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("order/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<SalesOrderFetchDto>(id)));
        }
        [HttpPost("order/closetransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CloseOrder(int id)
        {
            SalesOrderCloseDto command = new SalesOrderCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
        [HttpPost("billing/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllBilling([FromBody] FetchPagedListRequestDto<SalesBillingListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("billing/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTransBilling(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<SalesBillingFetchDto>(id)));
        }

        [HttpPost("billing")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertBillingSales([FromBody] SalesBillingInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("billing")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateBilling([FromBody] SalesBillingUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("billing/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteBilling(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<SalesBillingFetchDto>(id)));
        }
        [HttpPost("billing/closetransaction/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CloseBilling(int id)
        {
            SalesBillingCloseDto command = new SalesBillingCloseDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }
    }
}
