using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Donations;

namespace STTB.WebApiStandard.WebApi.Controllers
{
    [Route("api/v1/donations")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DonationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-donor-member")]
        public async Task<IActionResult> AddDonorMember([FromForm] AddDonorMemberRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }
    }
}
