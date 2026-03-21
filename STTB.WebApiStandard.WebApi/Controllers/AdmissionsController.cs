using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Admissions;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.WebApi.Controllers
{
    [Route("api/v1/admissions")]
    [ApiController]
    public class AdmissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdmissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-admission-schedule")]
        public async Task<IActionResult> GetAdmissionSchedule(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAdmissionScheduleRequest(), ct);
            return Ok(response);
        }

        [HttpGet("get-all-admission-costs")]
        public async Task<IActionResult> GetAllAdmissionCosts(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAllAdmissionCostRequest(), ct);
            return Ok(response);
        }
    }
}
