using MediatR;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms.ProgramCourses
{
    public class AddAcademicProgramCourseHandler : IRequestHandler<AddAcademicProgramCoursesRequest, AddAcademicProgramCoursesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddAcademicProgramCourseHandler> _logger;

        public AddAcademicProgramCourseHandler(SttbDbContext db, ILogger<AddAcademicProgramCourseHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddAcademicProgramCoursesResponse> Handle(AddAcademicProgramCoursesRequest request, CancellationToken ct)
        {
            var course = new AcademicProgramCourse
            {
                Name = request.CourseName,
                Credits = request.Credits,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.AcademicProgramCourses.AddAsync(course, ct);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully created AcademicProgramCourse {Id}.", course.Id);

            return new AddAcademicProgramCoursesResponse
            {
                Id = course.Id,
                CourseName = course.Name,
                Credits = course.Credits,
                Description = course.Description ?? string.Empty
            };
        }
    }
}
