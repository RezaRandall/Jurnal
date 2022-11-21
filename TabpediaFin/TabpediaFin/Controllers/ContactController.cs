using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.ContactAddressHandler;
using TabpediaFin.Handler.ContactHandler;
using TabpediaFin.Handler.ContactPersonHandler;

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
        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll([FromBody] QueryPagedListDto<contactlistDto> request)
        {
            QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
            reqsend.PageSize = request.PageSize;
            reqsend.PageNum = request.PageNum;
            reqsend.Search = request.Search;
            reqsend.SortBy = request.SortBy;
            reqsend.SortDesc = request.SortDesc;
            return Result(await _mediator.Send(reqsend));
        }

        [HttpPost("customer/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomer([FromBody] QueryPagedListDto<contactlistDto> request)
        {
            QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
            reqsend.PageSize = request.PageSize;
            reqsend.PageNum = request.PageNum;
            reqsend.Search = request.Search;
            reqsend.SortBy = request.SortBy;
            reqsend.SortDesc = request.SortDesc;
            reqsend.contacttype = "customer";
            return Result(await _mediator.Send(reqsend));
        }
        [HttpPost("vendor/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetVendor([FromBody] QueryPagedListDto<contactlistDto> request)
        {
            QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
            reqsend.PageSize = request.PageSize;
            reqsend.PageNum = request.PageNum;
            reqsend.Search = request.Search;
            reqsend.SortBy = request.SortBy;
            reqsend.SortDesc = request.SortDesc;
            reqsend.contacttype = "vendor";
            return Result(await _mediator.Send(reqsend));
        }
        [HttpPost("employee/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployee([FromBody] QueryPagedListDto<contactlistDto> request)
        {
            QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
            reqsend.PageSize = request.PageSize;
            reqsend.PageNum = request.PageNum;
            reqsend.Search = request.Search;
            reqsend.SortBy = request.SortBy;
            reqsend.SortDesc = request.SortDesc;
            reqsend.contacttype = "employee";
            return Result(await _mediator.Send(reqsend));
        }
        [HttpPost("other/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetOther([FromBody] QueryPagedListDto<contactlistDto> request)
        {
            QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
            reqsend.PageSize = request.PageSize;
            reqsend.PageNum = request.PageNum;
            reqsend.Search = request.Search;
            reqsend.SortBy = request.SortBy;
            reqsend.SortDesc = request.SortDesc;
            reqsend.contacttype = "other";
            return Result(await _mediator.Send(reqsend));
        }


        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomer(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactFetchDto>(id)));
        }

        [HttpGet("contactaddress/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAddress(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactAddressFetchDto>(id)));
        }
        [HttpPost("contactaddress")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertAddress([FromBody] ContactAddressInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("contactaddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] ContactAddressUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpGet("contactperson/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPerson(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactPersonFetchDto>(id)));
        }
        [HttpPost("contactperson")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertPerson([FromBody] ContactPersonInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("contactperson")]
        public async Task<IActionResult> UpdatePerson([FromBody] ContactPersonUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        public enum contacttypeenum
        {
            customer,
            vendor,
            employee,
            other
        }
    }
}
