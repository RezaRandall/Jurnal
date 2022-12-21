using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.StockHandler;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TabpediaFin.Controllers
{
    [Route("api/stockopname")]
    [ApiController]
    public class StockOpnameController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;
        public StockOpnameController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpPost("getstock")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetList(FetchPagedListStockItem<StockProductListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("gethistorystock/{productId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetListHistopry(int productId)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<HistoryStockDto>(productId)));   
        }


        //[HttpGet("{id}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> Get(int id)
        //{
        //    return Result(await _mediator.Send(new FetchByIdRequestDto<StockOpnameFetchDto>(id)));
        //}

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert([FromBody] StockOpnameInsertList command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] StockOpnameUpdateList command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<StockProductListDto>(id)));
        }
    }
}
