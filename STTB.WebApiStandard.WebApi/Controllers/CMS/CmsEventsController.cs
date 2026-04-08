using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/events")]
    [ApiController]
    //[Authorize]
    [Authorize(Policy = "CanManageEvents")]
    public class CmsEventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsEventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-events")]
        public async Task<IActionResult> GetAllEvents([FromQuery] GetAllEventRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-event")]
        public async Task<IActionResult> AddEvent([FromForm] AddEventRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-event/{id}")]
        public async Task<IActionResult> GetEvent(long id, CancellationToken ct)
        {
            var request = new GetEventRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-event")]
        public async Task<IActionResult> EditEvent([FromForm] EditEventRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-event/{id}")]
        public async Task<IActionResult> DeleteEvent(long id, CancellationToken ct)
        {
            var request = new DeleteEventRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }

        #region Categories
        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories([FromQuery] GetAllEventCategoriesRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> AddEventCategory([FromBody] AddEventCategoryRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> GetEventCategory(long id, CancellationToken ct)
        {
            var request = new GetEventCategoryRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-category")]
        public async Task<IActionResult> EditEventCategory([FromBody] EditEventCategoryRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteEventCategory(long id, CancellationToken ct)
        {
            var request = new DeleteEventCategoryRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
        #endregion
    }
}
