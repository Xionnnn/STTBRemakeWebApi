using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Media;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.WebApi.Controllers
{
    [Route("api/v1/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-available-media/{media_format}")]
        public async Task<IActionResult> GetAvailableMedia([FromRoute] string media_format, [FromQuery] GetAvailableMediaRequest request, CancellationToken ct)
        {
            request.MediaFormat = media_format;
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-media-categories")]
        public async Task<IActionResult> GetVideoCategories(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetMediaCategoriesRequest(), ct);
            return Ok(response);
        }

        [HttpGet("get-journal/{slug}")]
        public async Task<IActionResult> GetJournalDetail([FromRoute] string slug, CancellationToken ct)
        {
            var request = new GetJournalDetailRequest { JournalSlug = slug };
            var response = await _mediator.Send(request, ct);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("get-article/{slug}")]
        public async Task<IActionResult> GetArticleDetail([FromRoute] string slug, CancellationToken ct)
        {
            var request = new GetArticleDetailRequest { ArticleSlug = slug };
            var response = await _mediator.Send(request, ct);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("get-video/{slug}")]
        public async Task<IActionResult> GetVideoDetail([FromRoute] string slug, CancellationToken ct)
        {
            var request = new GetVideoDetailRequest { VideoSlug = slug };
            var response = await _mediator.Send(request, ct);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
