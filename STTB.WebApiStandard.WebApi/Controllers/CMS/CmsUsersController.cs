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
    }
}
