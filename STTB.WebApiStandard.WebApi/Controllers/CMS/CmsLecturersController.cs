using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/lecturers")]
    [ApiController]
    [Authorize(Policy = "CanManageLecturers")]
    public class CmsLecturersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsLecturersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-lecturers")]
        public async Task<IActionResult> GetAllLecturers([FromQuery] GetAllLecturerRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-lecturer/{id}")]
        public async Task<IActionResult> GetLecturer(long id, CancellationToken ct)
        {
            var request = new GetLecturerRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-lecturer")]
        public async Task<IActionResult> AddLecturer([FromForm] AddLecturerRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-lecturer")]
        public async Task<IActionResult> EditLecturer([FromForm] EditLecturerRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-lecturer/{id}")]
        public async Task<IActionResult> DeleteLecturer(long id, CancellationToken ct)
        {
            var request = new DeleteLecturerRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
    }
}
