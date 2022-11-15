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
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(IMediator mediator, ICurrentUser currentUser, ICustomerRepository customerRepository)
        {
            _mediator = mediator;
            _currentUser = currentUser;
            _customerRepository = customerRepository;
        }

        [HttpGet("getlistcustomer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getlistcustomer(string? sortby, string? valsort, string? searchby, string? valsearch, int? jumlah_data, int? offset)
        {
            var result = await _customerRepository.GetCustomerList(sortby, valsort, searchby, valsearch, jumlah_data, offset, _currentUser.TenantId);
            return Ok(result);
        }

        [HttpGet("getcustomer/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var result = await _customerRepository.GetCustomer(_currentUser.TenantId, id);
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> create([FromBody] contactpost customer)
        {
            var result = await _customerRepository.CreateCustomer(customer, _currentUser.TenantId, _currentUser.UserId);
            return Ok(result);
        }
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Put([FromBody] contactpost customer,int id)
        {
            var result = await _customerRepository.UpdateCustomer(customer, _currentUser.TenantId, _currentUser.UserId, id);

            return Ok(result);
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerRepository.DeleteCustomer(id, _currentUser.TenantId);
            return Ok(result);
        }


    }
}
