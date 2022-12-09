using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TabpediaFin.Handler.UploadAttachmentHandler;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TabpediaFin.Controllers
{
    [Route("api/attachment")]
    [ApiController]
    public class AttachmentController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AttachmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Insert(IFormFile FileAttachment)
        {
            requestupload command = new requestupload();
            command.fileupload = FileAttachment;
            return Result(await _mediator.Send(command));
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(deletefile command)
        {
            return Result(await _mediator.Send(command));
        }
    }
}
