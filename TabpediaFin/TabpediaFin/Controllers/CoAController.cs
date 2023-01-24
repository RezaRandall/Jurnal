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

        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetList([FromBody] QueryPagedListAccountDto<AccountListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpPost("getchildaccountlist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetListChild([FromBody] QueryPagedChildListAccountDto<AccountChildListDto> request)
        {
            return Result(await _mediator.Send(request));
        }


        [HttpGet("getaccountpurchasesales")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAccountPurchaseSales()
        {
            AccountPurchaseSalesList command = new AccountPurchaseSalesList();
            return Result(await _mediator.Send(command));
        }

        [HttpPost("generateaccountfirst")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GenerateAccountFirst()
        {
            GenerateAccountInsertDto command = new GenerateAccountInsertDto();
            return Result(await _mediator.Send(command));
        }

        public enum accounttype{
            all = 0,
            purchase = 1,
            sales = 2
        }
    }
}
