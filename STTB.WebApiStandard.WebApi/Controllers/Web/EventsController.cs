using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Events;

namespace STTB.WebApiStandard.WebApi.Controllers.Web
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-available-events")]
        public async Task<IActionResult> GetAvailable([FromQuery] GetAvailableEventRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-event/{slug}")]
        public async Task<IActionResult> GetEvent(string slug, CancellationToken ct)
        {
            var request = new GetEventRequest { EventSlug = slug };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-all-organizers")]
        public async Task<IActionResult> GetAllOrganizers(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAllOrganizersRequest(), ct);
            return Ok(response);
        }

        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAllEventCategoriesRequest(), ct);
            return Ok(response);
        }
    }
}
