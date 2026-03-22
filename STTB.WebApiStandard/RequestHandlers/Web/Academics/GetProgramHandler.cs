using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Academics;
using STTB.WebApiStandard.Contracts.ResponseModels.Academic;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Web.Academics
{
    public class GetProgramHandler : IRequestHandler<GetProgramRequest, GetProgramResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetProgramHandler> _logger;
        public GetProgramHandler(SttbDbContext db, ILogger<GetProgramHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetProgramResponse> Handle(GetProgramRequest request, CancellationToken ct)
        {
            var program = await _db.AcademicPrograms
                .Include(p => p.AcademicProgramRequirements)
                .Include(p => p.AcademicProgramNotes)
                .Include(p => p.AcademicProgramSystems)
                .Include(p => p.AcademicCourseCategories)
                    .ThenInclude(cc => cc.AcademicCourses)
                .Where(p => p.Slug == request.ProgramSlug && p.IsPublished)
                .AsNoTracking()
                .Select(p => new GetProgramResponse
                {
                    ProgramName = p.Name,
                    ProgramDescription = p.GraduateProfileDescription ?? string.Empty,
                    ProgramRequirements = p.AcademicProgramRequirements
                        .Select(r => r.RequirementText)
                        .ToList(),
                    TotalCredits = p.TotalCredits,
                    Duration = p.StudyDuration.ToString(),
                    Notes = p.AcademicProgramNotes
                        .Select(n => n.NoteText)
                        .ToList(),
                    LecturingSystem = p.AcademicProgramSystems
                        .Select(s => s.SystemText)
                        .ToList(),
                    Degree = p.DegreeAbbr,
                    Motto = p.GraduateProfileMotto,
                    InformedDescription = p.InformedDescription,
                    TransformedDescription = p.TransformedDescription,
                    TransformativeDescription = p.TransformativeDescription,
                    LectureCategory = p.AcademicCourseCategories
                        .Select(cc => new AcademicDTO
                        {
                            CategoryName = cc.Name,
                            TotalCredits = cc.TotalCredits,
                            Lectures = cc.AcademicCourses
                                .Select(c => new LectureDTO
                                {
                                    LectureName = c.Name,
                                    Credits = c.Credits,
                                    Description = c.Description ?? string.Empty
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (program == null)
            {
                _logger.LogInformation($"Program with slug '{request.ProgramSlug}' not found");
            }

            return program;
        }
    }
}
