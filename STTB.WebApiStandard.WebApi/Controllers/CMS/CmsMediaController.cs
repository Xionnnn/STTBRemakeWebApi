using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Videos;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [ApiController]
    [Route("api/v1/cms/media")]
    //[Authorize]
    [Authorize(Policy = "CanManageMedia")]
    public class CmsMediaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsMediaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllMedia([FromQuery] GetAllMediaRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        #region Categories
        [HttpGet("categories/get-all")]
        public async Task<IActionResult> GetAllMediaCategories([FromQuery] GetAllMediaCategoryRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request ?? new GetAllMediaCategoryRequest(), ct);
            return Ok(response);
        }

        [HttpPost("categories/add")]
        public async Task<IActionResult> AddMediaCategory([FromBody] AddMediaCategoryRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("categories/get/{id}")]
        public async Task<IActionResult> GetMediaCategory(long id, CancellationToken ct)
        {
            var request = new GetMediaCategoryRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("categories/edit")]
        public async Task<IActionResult> EditMediaCategory([FromBody] EditMediaCategoryRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("categories/delete/{id}")]
        public async Task<IActionResult> DeleteMediaCategory(long id, CancellationToken ct)
        {
            var request = new DeleteMediaCategoryRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
        #endregion

        #region Articles
        [HttpPost("articles/add")]
        public async Task<IActionResult> AddArticle([FromForm] AddMediaArticleRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("articles/edit/{id}")]
        public async Task<IActionResult> EditArticle([FromRoute] long id, [FromForm] EditMediaArticleRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("articles/get/{id}")]
        public async Task<IActionResult> GetArticleDetail([FromRoute] long id)
        {
            var response = await _mediator.Send(new GetMediaArticleRequest { Id = id });
            return Ok(response);
        }

        [HttpDelete("articles/delete/{id}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] long id)
        {
            await _mediator.Send(new DeleteMediaArticleRequest { Id = id });
            return NoContent();
        }
        #endregion

        #region Videos
        [HttpPost("videos/add")]
        public async Task<IActionResult> AddVideo([FromForm] AddMediaVideoRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("videos/edit/{id}")]
        public async Task<IActionResult> EditVideo([FromRoute] long id, [FromForm] EditMediaVideoRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("videos/get/{id}")]
        public async Task<IActionResult> GetVideoDetail([FromRoute] long id)
        {
            var response = await _mediator.Send(new GetMediaVideoRequest { Id = id });
            return Ok(response);
        }

        [HttpDelete("videos/delete/{id}")]
        public async Task<IActionResult> DeleteVideo([FromRoute] long id)
        {
            await _mediator.Send(new DeleteMediaVideoRequest { Id = id });
            return NoContent();
        }
        #endregion

        #region Journals
        [HttpPost("journals/add")]
        public async Task<IActionResult> AddJournal([FromForm] AddMediaJournalRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("journals/edit/{id}")]
        public async Task<IActionResult> EditJournal([FromRoute] long id, [FromForm] EditMediaJournalRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("journals/get/{id}")]
        public async Task<IActionResult> GetJournalDetail([FromRoute] long id)
        {
            var response = await _mediator.Send(new GetMediaJournalRequest { Id = id });
            return Ok(response);
        }

        [HttpDelete("journals/delete/{id}")]
        public async Task<IActionResult> DeleteJournal([FromRoute] long id)
        {
            await _mediator.Send(new DeleteMediaJournalRequest { Id = id });
            return NoContent();
        }
        #endregion

        #region Buletins
        [HttpPost("buletins/add")]
        public async Task<IActionResult> AddBuletin([FromForm] AddMediaBuletinRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("buletins/edit/{id}")]
        public async Task<IActionResult> EditBuletin([FromRoute] long id, [FromForm] EditMediaBuletinRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("buletins/get/{id}")]
        public async Task<IActionResult> GetBuletinDetail([FromRoute] long id)
        {
            var response = await _mediator.Send(new GetMediaBuletinRequest { Id = id });
            return Ok(response);
        }

        [HttpDelete("buletins/delete/{id}")]
        public async Task<IActionResult> DeleteBuletin([FromRoute] long id)
        {
            await _mediator.Send(new DeleteMediaBuletinRequest { Id = id });
            return NoContent();
        }
        #endregion

        #region Monografs
        [HttpPost("monografs/add")]
        public async Task<IActionResult> AddMonograf([FromForm] AddMediaMonografRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("monografs/edit/{id}")]
        public async Task<IActionResult> EditMonograf([FromRoute] long id, [FromForm] EditMediaMonografRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("monografs/get/{id}")]
        public async Task<IActionResult> GetMonografDetail([FromRoute] long id)
        {
            var response = await _mediator.Send(new GetMediaMonografRequest { Id = id });
            return Ok(response);
        }

        [HttpDelete("monografs/delete/{id}")]
        public async Task<IActionResult> DeleteMonograf([FromRoute] long id)
        {
            await _mediator.Send(new DeleteMediaMonografRequest { Id = id });
            return NoContent();
        }
        #endregion
    }
}
