using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Libraries;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.WebApi.Controllers
{
    [Route("api/v1/library")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibraryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-library-member")]
        public async Task<IActionResult> AddLibraryMember([FromForm] AddLibraryMemberRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }
    }
}
