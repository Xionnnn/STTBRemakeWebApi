using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms
{
    public class GetAcademicProgramHandler : IRequestHandler<GetAcademicProgramRequest, GetAcademicProgramResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAcademicProgramHandler> _logger;

        public GetAcademicProgramHandler(SttbDbContext db, ILogger<GetAcademicProgramHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAcademicProgramResponse> Handle(GetAcademicProgramRequest request, CancellationToken ct)
        {
            var program = await _db.AcademicPrograms
                .Include(p => p.AcademicProgramRequirements)
                .Include(p => p.AcademicProgramNotes)
                .Include(p => p.AcademicProgramSystems)
                .Include(p => p.AcademicCourseCategories)
                    .ThenInclude(c => c.AcademicCourses)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (program == null)
            {
                throw new KeyNotFoundException($"Academic Program with ID {request.Id} was not found.");
            }

            return new GetAcademicProgramResponse
            {
                Id = program.Id,
                ProgramName = program.Name,
                ProgramDescription = program.GraduateProfileDescription ?? string.Empty,
                ProgramRequirements = program.AcademicProgramRequirements.Select(r => r.RequirementText).ToList(),
                TotalCredits = program.TotalCredits,
                Duration = program.StudyDuration,
                Notes = program.AcademicProgramNotes.Select(n => n.NoteText).ToList(),
                LecturingSystem = program.AcademicProgramSystems.Select(s => s.SystemText).ToList(),
                Degree = program.DegreeAbbr,
                Motto = program.GraduateProfileMotto,
                InformedDescription = program.InformedDescription,
                TransformedDescription = program.TransformedDescription,
                TransformativeDescription = program.TransformativeDescription,
                LectureCategory = program.AcademicCourseCategories.Select(c => new AcademicDTO
                {
                    CategoryName = c.Name,
                    TotalCredits = c.TotalCredits,
                    Lectures = c.AcademicCourses.Select(course => new LectureDTO
                    {
                        LectureName = course.Name,
                        Credits = course.Credits,
                        Description = course.Description ?? string.Empty
                    }).ToList()
                }).ToList()
            };
        }
    }
}
