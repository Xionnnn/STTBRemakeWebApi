using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Academic;

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
    }
}
