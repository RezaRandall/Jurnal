using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TabpediaFin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;
        private readonly IVendorRepository _vendorRepository;
        public VendorController(IMediator mediator, ICurrentUser currentUser, IVendorRepository vendorRepository)
        {
            _mediator = mediator;
            _currentUser = currentUser;
            _vendorRepository = vendorRepository;
        }

        [HttpGet("getlistvendor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getlistcustomer(string? sortby, string? valsort, string? searchby, string? valsearch, int? jumlah_data, int? offset)
        {
            var result = await _vendorRepository.GetVendorList(sortby, valsort, searchby, valsearch, jumlah_data, offset, _currentUser.TenantId);
            return Ok(result);
        }

        [HttpGet("getvendor/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var result = await _vendorRepository.GetVendor(_currentUser.TenantId, id);
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> create([FromBody] contactpost customer)
        {
            var result = await _vendorRepository.CreateVendor(customer, _currentUser.TenantId, _currentUser.UserId);
            return Ok(result);
        }
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Put([FromBody] contactpost customer, int id)
        {
            var result = await _vendorRepository.UpdateVendor(customer, _currentUser.TenantId, _currentUser.UserId, id);

            return Ok(result);
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _vendorRepository.DeleteVendor(id, _currentUser.TenantId);
            return Ok(result);
        }

    }
}
