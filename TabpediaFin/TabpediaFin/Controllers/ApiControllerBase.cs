using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Dto.Common.Response;

namespace TabpediaFin.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public abstract class ApiControllerBase : ControllerBase
{
    protected ApiControllerBase()
    {
    }


    protected IActionResult Result(BaseResponse result)
    {
        if (result.IsOk)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

}
