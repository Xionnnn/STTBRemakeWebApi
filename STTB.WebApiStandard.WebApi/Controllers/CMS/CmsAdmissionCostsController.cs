using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/costs")]
    [ApiController]
    [Authorize(Policy = "CanManageAdmissionCost")]
    //[Authorize]
    public class CmsAdmissionCostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsAdmissionCostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-costs")]
        public async Task<IActionResult> GetAllCosts([FromQuery] GetAllCostRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-cost/{id}")]
        public async Task<IActionResult> GetCost(long id, CancellationToken ct)
        {
            var request = new GetCostRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-cost")]
        public async Task<IActionResult> AddCost([FromBody] AddCostRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-cost")]
        public async Task<IActionResult> EditCost([FromBody] EditCostRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-cost/{id}")]
        public async Task<IActionResult> DeleteCost(long id, CancellationToken ct)
        {
            var request = new DeleteCostRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }

        #region Categories
        [HttpGet("categories/get-all")]
        public async Task<IActionResult> GetAllCategories([FromQuery] GetAllCostCategoryRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request ?? new GetAllCostCategoryRequest(), ct);
            return Ok(response);
        }

        [HttpPost("categories/add")]
        public async Task<IActionResult> AddCategory([FromBody] AddCostCategoryRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("categories/get/{id}")]
        public async Task<IActionResult> GetCategory(long id, CancellationToken ct)
        {
            var request = new GetCostCategoryRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("categories/edit")]
        public async Task<IActionResult> EditCategory([FromBody] EditCostCategoryRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("categories/delete/{id}")]
        public async Task<IActionResult> DeleteCategory(long id, CancellationToken ct)
        {
            var request = new DeleteCostCategoryRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }
        #endregion
    }
}
