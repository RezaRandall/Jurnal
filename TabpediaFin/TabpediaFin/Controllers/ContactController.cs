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
        
        [HttpPost("list/{contacttype}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomer(contacttypeenum contacttype, [FromBody] QueryPagedListDto<contactlistDto> request)
        {
            QueryPagedListContactDto<contactlistDto> reqsend = new QueryPagedListContactDto<contactlistDto>();
            reqsend.PageSize = request.PageSize;
            reqsend.PageNum = request.PageNum;
            reqsend.Search = request.Search;
            reqsend.SortBy = request.SortBy;
            reqsend.SortDesc = request.SortDesc;
            reqsend.contacttype = contacttype.ToString();
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

        [HttpGet("contactperson/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPerson(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ContactPersonFetchDto>(id)));
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
