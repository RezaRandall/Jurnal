using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.ItemCategoryHandler;

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

        [HttpPost("list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetList([FromBody] QueryPagedListDto<ItemCategoryListDto> request)
        {
            return Result(await _mediator.Send(request));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(int id)
        {
            return Result(await _mediator.Send(new QueryByIdDto<ItemCategoryFetchDto>(id)));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert([FromBody] ItemCategoryInsertDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] ItemCategoryUpdateDto command)
        {
            return Result(await _mediator.Send(command));
        }

        [HttpDelete("delete/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
