using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/administrators")]
    [ApiController]
    [Authorize(Policy = "CanManageAdministrator")]
    public class CmsAdministratorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsAdministratorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-administrators")]
        public async Task<IActionResult> GetAllAdministrators([FromQuery] GetAllAdministratorRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-administrator/{id}")]
        public async Task<IActionResult> GetAdministrator(long id, CancellationToken ct)
        {
            var request = new GetAdministratorRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-administrator")]
        public async Task<IActionResult> AddAdministrator([FromBody] AddAdministratorRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-administrator")]
        public async Task<IActionResult> EditAdministrator([FromBody] EditAdministratorRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-administrator/{id}")]
        public async Task<IActionResult> DeleteAdministrator(long id, CancellationToken ct)
        {
            var request = new DeleteAdministratorRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
    }
}
