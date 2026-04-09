using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.DTOs.Web.Academics;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Academics;
using STTB.WebApiStandard.Contracts.ResponseModels.Academic;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Web.Academics
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
                .Include(p => p.AcademicProgramCategories)
                    .ThenInclude(cc => cc.AcademicCategoryCourses)
                .Where(p => p.Slug == request.ProgramSlug && p.IsPublished)
                .AsNoTracking()
                .Select(p => new GetAcademicProgramResponse
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
                    ProgramCategory = p.AcademicProgramCategories
                        .Select(c => new AcademicCategoryDTO
                        {
                            CategoryName = c.Name,
                            TotalCredits = c.TotalCredits,
                            Courses = c.AcademicCategoryCourses
                                .Select(c => new AcademicCourseDTO
                                {
                                    CourseName = c.AcademicProgramCourse.Name,
                                    Credits = c.AcademicProgramCourse.Credits,
                                    Description = c.AcademicProgramCourse.Description ?? string.Empty
                                })
                                .ToList()
                        }).ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (program == null)
            {
                _logger.LogInformation($"Program with slug '{request.ProgramSlug}' not found");
                throw new Exception("Data doesnt exist");
            }

            return program;
        }
    }
}
