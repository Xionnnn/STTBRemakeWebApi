using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/news")]
    [ApiController]
    //[Authorize]
    [Authorize(Policy ="CanManageNews")]
    public class CmsNewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsNewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-news")]
        public async Task<IActionResult> GetAllNews([FromQuery] GetAllNewsRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAllNewsCategoriesRequest(), ct);
            return Ok(response);
        }

        [HttpPost("add-news")]
        public async Task<IActionResult> AddNews([FromForm] AddNewsRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-news/{id}")]
        public async Task<IActionResult> GetNews(long id, CancellationToken ct)
        {
            var request = new GetNewsRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-news")]
        public async Task<IActionResult> EditNews([FromForm] EditNewsRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-news/{id}")]
        public async Task<IActionResult> DeleteNews(long id, CancellationToken ct)
        {
            var request = new DeleteNewsRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
    }
}
