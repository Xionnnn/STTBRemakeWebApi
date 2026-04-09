using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms.ProgramCourses
{
    public class GetAcademicProgramCourseHandler : IRequestHandler<GetAcademicProgramCoursesRequest, GetAcademicProgramCoursesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAcademicProgramCourseHandler> _logger;

        public GetAcademicProgramCourseHandler(SttbDbContext db, ILogger<GetAcademicProgramCourseHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAcademicProgramCoursesResponse> Handle(GetAcademicProgramCoursesRequest request, CancellationToken ct)
        {
            var course = await _db.AcademicProgramCourses
                .AsNoTracking()
                .Select(c => new GetAcademicProgramCoursesResponse
                {
                    Id = c.Id,
                    CourseName = c.Name,
                    Credits = c.Credits,
                    Description = c.Description ?? string.Empty
                })
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            _logger.LogInformation("Retrieved AcademicProgramCourse {Id}. Found: {Found}", request.Id, course != null);

            return course!;
        }
    }
}
