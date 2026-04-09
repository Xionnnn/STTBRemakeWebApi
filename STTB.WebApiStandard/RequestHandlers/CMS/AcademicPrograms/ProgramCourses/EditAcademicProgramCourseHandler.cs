using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms.ProgramCourses
{
    public class EditAcademicProgramCourseHandler : IRequestHandler<EditAcademicProgramCoursesRequest, EditAcademicProgramCoursesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditAcademicProgramCourseHandler> _logger;

        public EditAcademicProgramCourseHandler(SttbDbContext db, ILogger<EditAcademicProgramCourseHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditAcademicProgramCoursesResponse> Handle(EditAcademicProgramCoursesRequest request, CancellationToken ct)
        {
            var course = await _db.AcademicProgramCourses
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (course == null)
            {
                throw new Exception("Data doesnt exist");
            }

            course.Name = request.CourseName;
            course.Credits = request.Credits;
            course.Description = request.Description;
            course.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully updated AcademicProgramCourse {Id}.", request.Id);

            return new EditAcademicProgramCoursesResponse
            {
                Id = request.Id,
                CourseName = request.CourseName,
                Credits = request.Credits,
                Description = request.Description
            };
        }
    }
}
