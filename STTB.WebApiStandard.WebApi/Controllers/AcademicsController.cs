using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Academic;

using STTB.WebApiStandard.Contracts.RequestModels.Academics;

namespace STTB.WebApiStandard.WebApi.Controllers
{
    [Route("api/v1/academics")]
    [ApiController]
    public class AcademicsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AcademicsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-available-programs")]
        public async Task<IActionResult> GetAvailablePrograms([FromQuery] GetAvailableProgramRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-program/{slug}")]
        public async Task<IActionResult> GetProgram(string slug, CancellationToken ct)
        {
            var request = new GetProgramRequest { ProgramSlug = slug };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-academic-requirements")]
        public async Task<IActionResult> GetAcademicRequirements(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAcademicRequirementsRequest(), ct);
            return Ok(response);
        }
    }
}
