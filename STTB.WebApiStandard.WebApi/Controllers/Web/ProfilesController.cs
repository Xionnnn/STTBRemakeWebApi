using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Profiles;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.WebApi.Controllers.Web
{
    [Route("api/v1/profiles")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-lecturers")]
        public async Task<IActionResult> GetAllLecturer(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAllLecturerRequest(), ct);
            return Ok(response);
        }

        [HttpGet("get-all-administrators")]
        public async Task<IActionResult> GetAllAdministrator(CancellationToken ct)
        {
            var response = await _mediator.Send(new GetAllAdministratorrequest(), ct);
            return Ok(response);
        }
    }
}
