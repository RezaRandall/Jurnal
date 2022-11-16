using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TabpediaFin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;
        public CustomerController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("getlistcustomer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getlistcustomer([FromQuery] string? sortby, [FromQuery] string? valsort, [FromQuery] string? searchby, [FromQuery] string? valsearch, [FromQuery] int? jumlah_data, [FromQuery] int? offset)
        {
            GetCustomerListQuery param = new GetCustomerListQuery();
            param.sortby = sortby;
            param.valsort = valsort;
            param.searchby = searchby;
            param.valsearch = valsearch;
            param.jumlah_data = jumlah_data;
            param.offset = offset;
            param.TenantId = _currentUser.TenantId;
            var result = await _mediator.Send(param);
            return Ok(result);
        }

        [HttpGet("getcustomer/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomer(int id)
        {
            GetCustomerQuery param = new GetCustomerQuery();
            param.Id = id;
            param.TenantId = _currentUser.TenantId;
            var result = await _mediator.Send(param);
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> create([FromBody] AddCustomer customer)
        {
            customer.TenantId = _currentUser.TenantId;
            customer.CreatedUid = _currentUser.UserId;
            var result = await _mediator.Send(customer);
            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put([FromBody] UpdateCustomer customer)
        {
            customer.TenantId = _currentUser.TenantId;
            customer.UpdatedUid = _currentUser.UserId;
            var result = await _mediator.Send(customer);
            return Ok(result);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            customrespons response = new customrespons();
            response.status = "failed";
            response.message = "Data not found";
            DeleteCustomer param = new DeleteCustomer();
            param.Id = id;
            param.TenantId = _currentUser.TenantId;
            var result = await _mediator.Send(param);
            if (result == true)
            {
                response.status = "success";
                response.message = "contact customer with id "+ id +" was deleted";
            }
            return Ok(response);
        }


    }
}
