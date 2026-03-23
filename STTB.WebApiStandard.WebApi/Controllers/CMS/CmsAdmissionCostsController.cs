using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/costs")]
    [ApiController]
    [Authorize(Policy = "CanManageAdmissionCost")]
    public class CmsAdmissionCostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsAdmissionCostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-costs")]
        public async Task<IActionResult> GetAllCosts([FromQuery] GetAllCostRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-cost/{id}")]
        public async Task<IActionResult> GetCost(long id, CancellationToken ct)
        {
            var request = new GetCostRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-cost")]
        public async Task<IActionResult> EditCost([FromBody] EditCostRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-cost/{id}")]
        public async Task<IActionResult> DeleteCost(long id, CancellationToken ct)
        {
            var request = new DeleteCostRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
    }
}
