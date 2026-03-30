using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;

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

        // ─── Roles ────────────────────────────────────────────────────

        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAllRoles([FromQuery] GetAllUserRoleRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-role/{id}")]
        public async Task<IActionResult> DeleteRole(long id, CancellationToken ct)
        {
            var request = new DeleteUserRoleRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }

        // ─── Permissions ──────────────────────────────────────────────

        [HttpGet("get-all-permissions")]
        public async Task<IActionResult> GetAllPermissions([FromQuery] GetAllPermissionRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-permission")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-permission/{id}")]
        public async Task<IActionResult> DeletePermission(long id, CancellationToken ct)
        {
            var request = new DeletePermissionRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
    }
}
