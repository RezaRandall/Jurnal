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

        [HttpGet("accountcategory/{idaccount:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetListCategory(int idaccount)
        {
            return Result(await _mediator.Send(new ListByIdRequestDto<AccountCategoryDto>(idaccount)));
        }
        
        [HttpGet("createdefaultnumber/{idcategory:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetNumberCategory(int idcategory)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<GenerateNumberDto>(idcategory)));
        }


        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetList([FromBody] QueryPagedListAccountDto<AccountListDto> request)
        {
            return Result(await _mediator.Send(request));
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCOAId(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<AccountCoAFetchDto>(id)));
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

        [HttpGet("getcoa/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCOAListId(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<AccountCoAFetchDto>(id)));
        }
    }
}
