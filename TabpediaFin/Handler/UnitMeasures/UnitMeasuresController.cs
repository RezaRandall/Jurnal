using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Domain;

namespace TabpediaFin.Handler.UnitMeasures
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitMeasuresController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;

        public UnitMeasuresController(
            IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet]
        //public async Task<IEnumerable<UnitMeasureDto>> GetAllUnitMeasure() => await _mediator.Send(new UnitMeasureList.GetAllUnitMeasure());
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public UnitMeasureDto GetAllUnitMeasure()
        {
            if (_currentUser == null) return new UnitMeasureDto();

            return new UnitMeasureDto()
            {
                Id = _currentUser.TenantId,
                TenantId = _currentUser.TenantId,

                //Name = _currentuser.N ?? string.empty,
                //Description = _currentuser.desc ?? string.empty
            };
        }


    }
}
