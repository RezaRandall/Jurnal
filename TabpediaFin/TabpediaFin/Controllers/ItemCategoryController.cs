using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TabpediaFin.Controllers
{
    [Route("api/itemcategory")]
    [ApiController]
    public class ItemCategoryController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;
        public ItemCategoryController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("getlistitemcategory")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getlistitemcategory([FromQuery] string? sortby, [FromQuery] string? valsort, [FromQuery] string? searchby, [FromQuery] string? valsearch, [FromQuery] int? jumlah_data, [FromQuery] int? offset)
        {
            GetItemCategoryListQuery param = new GetItemCategoryListQuery();
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

        
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetItemCategory(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ItemCategoryDto>(id)));
        }

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> create([FromBody] ItemCategory request)
        {
            AddItemCategory customer = new AddItemCategory();
            customer.Name = request.Name;
            customer.Description = request.Description;
            customer.TenantId = _currentUser.TenantId;
            customer.CreatedUid = _currentUser.UserId;
            var result = await _mediator.Send(customer);
            return Ok(result);
        }

        [HttpPut("update/{Id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(int Id, [FromBody] ItemCategory request)
        {
            UpdateItemCategory customer = new UpdateItemCategory();
            customer.Id = Id;
            customer.Name = request.Name;
            customer.Description = request.Description;
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
                response.message = "Item Category with id " + id + " was deleted";
            }
            return Ok(response);
        }
    }
}
