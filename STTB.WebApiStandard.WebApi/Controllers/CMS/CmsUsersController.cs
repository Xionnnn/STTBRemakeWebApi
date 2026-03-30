using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/users")]
    [ApiController]
    [Authorize]
    public class CmsUsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllIUserRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUser(long id, CancellationToken ct)
        {
            var request = new GetUserRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-user")]
        public async Task<IActionResult> EditUser([FromBody] EditUserRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(long id, CancellationToken ct)
        {
            var request = new DeleteUserRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
    }
}
