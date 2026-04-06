using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Dashboard;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [ApiController]
    [Route("api/v1/cms/dashboards")]
    //[Authorize]
    [Authorize(Policy = "CanViewDashboard")]
    public class CmsDashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsDashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData([FromQuery] GetDashboardDataRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
