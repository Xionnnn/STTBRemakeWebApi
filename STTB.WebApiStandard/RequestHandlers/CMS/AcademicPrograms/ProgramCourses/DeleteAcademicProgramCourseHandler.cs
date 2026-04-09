using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms.ProgramCourses
{
    public class DeleteAcademicProgramCourseHandler : IRequestHandler<DeleteAcademicProgramCoursesRequest, DeleteAcademicProgramCoursesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteAcademicProgramCourseHandler> _logger;

        public DeleteAcademicProgramCourseHandler(SttbDbContext db, ILogger<DeleteAcademicProgramCourseHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteAcademicProgramCoursesResponse> Handle(DeleteAcademicProgramCoursesRequest request, CancellationToken ct)
        {
            var course = await _db.AcademicProgramCourses
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (course == null)
            {
                throw new Exception("Data doesnt exist");
            }

            _db.AcademicProgramCourses.Remove(course);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully deleted AcademicProgramCourse {Id}.", request.Id);

            return new DeleteAcademicProgramCoursesResponse();
        }
    }
}
