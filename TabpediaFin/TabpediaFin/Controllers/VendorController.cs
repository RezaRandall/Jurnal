//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace TabpediaFin.Controllers
//{
//    [Route("api/vendor")]
//    [ApiController]
//    public class VendorController : ApiControllerBase
//    {
//        private readonly IMediator _mediator;
//        private readonly ICurrentUser _currentUser;
//        private readonly IVendorRepository _vendorRepository;
//        public VendorController(IMediator mediator, ICurrentUser currentUser, IVendorRepository vendorRepository)
//        {
//            _mediator = mediator;
//            _currentUser = currentUser;
//            _vendorRepository = vendorRepository;
//        }

//        [HttpGet("getlistvendor")]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public async Task<IActionResult> getlistvendor([FromQuery] string? sortby, [FromQuery] string? valsort, [FromQuery] string? searchby, [FromQuery] string? valsearch, [FromQuery] int? jumlah_data, [FromQuery] int? offset)
//        {
//            GetVendorListQuery param = new GetVendorListQuery();
//            param.sortby = sortby;
//            param.valsort = valsort;
//            param.searchby = searchby;
//            param.valsearch = valsearch;
//            param.jumlah_data = jumlah_data;
//            param.offset = offset;
//            param.TenantId = _currentUser.TenantId;
//            var result = await _mediator.Send(param);
//            return Ok(result);
//        }

//        [HttpGet("{id:int}")]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public async Task<IActionResult> GetVendor(int id)
//        {
//            return Result(await _mediator.Send(new QueryByIdDto<ContactDto>(id)));
//        }

//        [HttpPost("create")]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public async Task<IActionResult> create([FromBody] contactpost request)
//        {
//            AddVendor customer = new AddVendor();
//            customer.Name = request.Name;
//            customer.Address = request.Address;
//            customer.CityName = request.CityName;
//            customer.PostalCode = request.PostalCode;
//            customer.Email = request.Email;
//            customer.Phone = request.Phone;
//            customer.Fax = request.Fax;
//            customer.Website = request.Website;
//            customer.Npwp = request.Npwp;
//            customer.GroupId = request.GroupId;
//            customer.Notes = request.Notes;
//            customer.TenantId = _currentUser.TenantId;
//            customer.CreatedUid = _currentUser.UserId;
//            var result = await _mediator.Send(customer);
//            return Ok(result);
//        }
//        [HttpPut("update/{Id:int}")]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public async Task<IActionResult> Put(int Id,[FromBody] contactpost request)
//        {
//            UpdateVendor customer = new UpdateVendor();
//            customer.Id = Id;
//            customer.Name = request.Name;
//            customer.Address = request.Address;
//            customer.CityName = request.CityName;
//            customer.PostalCode = request.PostalCode;
//            customer.Email = request.Email;
//            customer.Phone = request.Phone;
//            customer.Fax = request.Fax;
//            customer.Website = request.Website;
//            customer.Npwp = request.Npwp;
//            customer.GroupId = request.GroupId;
//            customer.Notes = request.Notes;
//            customer.TenantId = _currentUser.TenantId;
//            customer.UpdatedUid = _currentUser.UserId;
//            var result = await _mediator.Send(customer);
//            return Ok(result);
//        }

//        [HttpDelete("delete/{id:int}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            customrespons response = new customrespons();
//            response.status = "failed";
//            response.message = "Data not found";
//            DeleteVendor param = new DeleteVendor();
//            param.Id = id;
//            param.TenantId = _currentUser.TenantId;
//            var result = await _mediator.Send(param);
//            if (result == true)
//            {
//                response.status = "success";
//                response.message = "contact vendor with id " + id + " was deleted";
//            }
//            return Ok(response);
//        }

//    }
//}
