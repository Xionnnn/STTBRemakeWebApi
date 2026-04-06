using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/admission-deadlines")]
    [ApiController]
    //[Authorize]
    [Authorize(Policy = "CanManageAdmissionsDeadline")]
    public class CmsAdmissionDeadlinesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsAdmissionDeadlinesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-batch-deadlines")]
        public async Task<IActionResult> GetAllBatchDeadlines([FromQuery] GetAllBatchDeadlineRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-batch-deadline/{id}")]
        public async Task<IActionResult> GetBatchDeadline(long id, CancellationToken ct)
        {
            var request = new GetBatchDeadlineRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-batch-deadline")]
        public async Task<IActionResult> EditBatchDeadline([FromBody] EditBatchDeadlineRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-batch-deadline/{id}")]
        public async Task<IActionResult> DeleteBatchDeadline(long id, CancellationToken ct)
        {
            var request = new DeleteBatchDeadlineRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
    }
}
