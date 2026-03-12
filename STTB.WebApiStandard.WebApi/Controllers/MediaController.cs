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

        [HttpGet("get-available-videos")]
        public async Task<IActionResult> GetAvailableVideos([FromQuery] GetAvailableVideoRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-video-categories")]
        public async Task<IActionResult> GetVideoCategories(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetVideoCategoriesRequest(), ct);
            return Ok(response);
        }
    }
}
