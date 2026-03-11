using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.News;

namespace STTB.WebApiStandard.WebApi.Controllers
{
    [Route("api/v1/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-available-news")]
        public async Task<IActionResult> GetAvailable([FromQuery] GetAvailableNewsRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }
    }
}
