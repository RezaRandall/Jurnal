using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TabpediaFin.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        //[HttpGet()]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> GetCustomer([FromQuery] string? sortby, [FromQuery] string? valsort, [FromQuery] string? searchby, [FromQuery] string? valsearch, [FromQuery] int? jumlah_data, [FromQuery] int? offset)
        //{
        //    GetCustomerListQuery param = new GetCustomerListQuery();
        //    param.sortby = sortby;
        //    param.valsort = valsort;
        //    param.searchby = searchby;
        //    param.valsearch = valsearch;
        //    param.jumlah_data = jumlah_data;
        //    param.offset = offset;

        //    var result = await _mediator.Send(param);
        //    return Ok(result);
        //}
        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomer([FromBody] QueryPagedListDto<contactqueryDto> request)
        {
            return Result(await _mediator.Send(request));
        }
        

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomer(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactDto>(id)));
        }

        [HttpGet("contactaddress/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAddress(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactAddressDto>(id)));
        }

        [HttpGet("contactperson/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPerson(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactPersonDto>(id)));
        }
        enum contacttype
        {
            Customer,
            Vendor,
            Employee,
            Other
        }
    }
}
