using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Web.News;

namespace STTB.WebApiStandard.WebApi.Controllers.Web
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

        [HttpGet("get-news/{slug}")]
        public async Task<IActionResult> GetNews(string slug, CancellationToken ct)
        {
            var request = new GetNewsRequest { NewsSlug = slug };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAllNewsCategoriesRequest(), ct);
            return Ok(response);
        }
    }
}
