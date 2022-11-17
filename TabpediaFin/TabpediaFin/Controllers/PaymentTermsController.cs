using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using static TabpediaFin.Dto.UnitMeasureDto;

namespace TabpediaFin.Controllers;

[Route("payment-term")]
public class PaymentTermController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public PaymentTermController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet("getAllListPaymentTerm")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> getAllListPaymentTerm(
         [FromQuery] string? searchby
        )
    {
        GetPaymentTermListQuery param = new GetPaymentTermListQuery();
        param.searchby = searchby;
        param.TenantId = _currentUser.TenantId;
        var result = await _mediator.Send(param);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Result(await _mediator.Send(new QueryByIdDto<PaymentTermDto>(id)));
    }

    [HttpPost("PaymentTerm")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> create([FromBody] AddPaymentTerm unitMeasure)
    {
        unitMeasure.TenantId = _currentUser.TenantId;
        unitMeasure.CreatedUid = _currentUser.UserId;
        var result = await _mediator.Send(unitMeasure);
        return Ok(result);
    }

    [HttpPut("updatePaymentTerm")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Put([FromBody] UpdatePaymentTerm unitMeasure)
    {
        unitMeasure.TenantId = _currentUser.TenantId;
        unitMeasure.UpdatedUid = _currentUser.UserId;
        var result = await _mediator.Send(unitMeasure);
        return Ok(result);
    }

    [HttpDelete("deletePaymentTerm/{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id)
    {
        PaymentTermRespons response = new PaymentTermRespons();
        //response.status = "failed";
        //response.message = "Data not found";
        DeletePaymentTerm param = new DeletePaymentTerm();
        param.Id = id;
        param.TenantId = _currentUser.TenantId;
        var result = await _mediator.Send(param);
        if (result == true)
        {
            response.status = "success";
            response.message = "payment term with id " + id + " was deleted";
        }
        else
        {
            response.status = "failed";
            response.message = "Data not found";
        }
        return Ok(response);
    }

}
