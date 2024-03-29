﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        public async Task<IActionResult> GetAll([FromBody] FetchPagedListRequestDto<contactlistDto> request)
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
        public async Task<IActionResult> GetCustomer([FromBody] FetchPagedListRequestDto<contactlistDto> request)
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
        public async Task<IActionResult> GetVendor([FromBody] FetchPagedListRequestDto<contactlistDto> request)
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
        public async Task<IActionResult> GetEmployee([FromBody] FetchPagedListRequestDto<contactlistDto> request)
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
        public async Task<IActionResult> GetOther([FromBody] FetchPagedListRequestDto<contactlistDto> request)
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
            return Result(await _mediator.Send(new FetchByIdRequestDto<ContactFetchDto>(id)));
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertContact([FromBody] ContactInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateContact([FromBody] ContactUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteContact(int id)
        {

            ContactDeleteDto command = new ContactDeleteDto();
            command.Id = id;
            return Result(await _mediator.Send(command));
        }

        [HttpGet("contactaddress/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAddress(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<ContactAddressFetchDto>(id)));
        }
        [HttpPost("contactaddress")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertAddress([FromBody] ContactAddressInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("contactaddress")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateAddress([FromBody] ContactAddressUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        
        [HttpDelete("contactaddress/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteContactAddress(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<ContactAddressFetchDto>(id)));
        }

        [HttpGet("contactperson/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPerson(int id)
        {
            return Result(await _mediator.Send(new FetchByIdRequestDto<ContactPersonFetchDto>(id)));
        }
        [HttpPost("contactperson")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> InsertPerson([FromBody] ContactPersonInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut("contactperson")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePerson([FromBody] ContactPersonUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }
        [HttpDelete("contactperson/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteContactPerson(int id)
        {
            return Result(await _mediator.Send(new DeleteByIdRequestDto<ContactPersonFetchDto>(id)));
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
