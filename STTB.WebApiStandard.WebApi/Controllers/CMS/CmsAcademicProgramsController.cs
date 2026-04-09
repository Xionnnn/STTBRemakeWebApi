using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;

namespace STTB.WebApiStandard.WebApi.Controllers.CMS
{
    [Route("api/v1/cms/academic-programs")]
    [ApiController]
    //[Authorize]
    [Authorize("CanManageAcademicPrograms")]
    public class CmsAcademicProgramsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CmsAcademicProgramsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-academic-programs")]
        public async Task<IActionResult> GetAllAcademicPrograms([FromQuery] GetAllAcademicProgramRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-academic-program/{id}")]
        public async Task<IActionResult> GetAcademicProgram(long id, CancellationToken ct)
        {
            var request = new GetAcademicProgramRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-academic-program")]
        public async Task<IActionResult> AddAcademicProgram([FromBody] AddAcademicProgramRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-academic-program")]
        public async Task<IActionResult> EditAcademicProgram([FromBody] EditAcademicProgramRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-academic-program/{id}")]
        public async Task<IActionResult> DeleteAcademicProgram(long id, CancellationToken ct)
        {
            var request = new DeleteAcademicProgramRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }

        #region Courses

        [HttpGet("get-all-courses")]
        public async Task<IActionResult> GetAllCourses([FromQuery] GetAllAcademicProgramCoursesRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpGet("get-course/{id}")]
        public async Task<IActionResult> GetCourse(long id, CancellationToken ct)
        {
            var request = new GetAcademicProgramCoursesRequest { Id = id };
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPost("add-course")]
        public async Task<IActionResult> AddCourse([FromBody] AddAcademicProgramCoursesRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpPut("edit-course")]
        public async Task<IActionResult> EditCourse([FromBody] EditAcademicProgramCoursesRequest request, CancellationToken ct)
        {
            var response = await _mediator.Send(request, ct);
            return Ok(response);
        }

        [HttpDelete("delete-course/{id}")]
        public async Task<IActionResult> DeleteCourse(long id, CancellationToken ct)
        {
            var request = new DeleteAcademicProgramCoursesRequest { Id = id };
            await _mediator.Send(request, ct);
            return NoContent();
        }

        #endregion
    }
}
