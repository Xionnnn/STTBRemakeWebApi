using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Entities;
using System.Text.RegularExpressions;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms
{
    public class EditAcademicProgramHandler : IRequestHandler<EditAcademicProgramRequest, EditAcademicProgramResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditAcademicProgramHandler> _logger;

        public EditAcademicProgramHandler(SttbDbContext db, ILogger<EditAcademicProgramHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditAcademicProgramResponse> Handle(EditAcademicProgramRequest request, CancellationToken ct)
        {
            var program = await _db.AcademicPrograms
                .Include(p => p.AcademicProgramRequirements)
                .Include(p => p.AcademicProgramNotes)
                .Include(p => p.AcademicProgramSystems)
                .Include(p => p.AcademicCourseCategories)
                    .ThenInclude(c => c.AcademicCourses)
                .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (program == null)
            {
                throw new KeyNotFoundException($"Academic Program with ID {request.Id} was not found.");
            }

            var newSlug = GenerateSlug(request.ProgramName);

            // Update basic fields
            program.Name = request.ProgramName;
            program.Slug = newSlug;
            program.GraduateProfileDescription = request.ProgramDescription;
            program.DegreeAbbr = request.Degree;
            program.GraduateProfileMotto = request.Motto;
            program.InformedDescription = request.InformedDescription;
            program.TransformedDescription = request.TransformedDescription;
            program.TransformativeDescription = request.TransformativeDescription;
            program.TotalCredits = request.TotalCredits ?? 0;
            program.IsPublished = request.IsPublished;
            program.UpdatedAt = DateTime.UtcNow;

            if (request.Duration.HasValue)
            {
                program.StudyDuration = request.Duration.Value;
            }

            // Remove existing collections
            _db.AcademicProgramRequirements.RemoveRange(program.AcademicProgramRequirements);
            _db.AcademicProgramNotes.RemoveRange(program.AcademicProgramNotes);
            _db.AcademicProgramSystems.RemoveRange(program.AcademicProgramSystems);
            
            foreach (var cat in program.AcademicCourseCategories)
            {
                _db.AcademicCourses.RemoveRange(cat.AcademicCourses);
            }
            _db.AcademicCourseCategories.RemoveRange(program.AcademicCourseCategories);

            // Rebuild collections
            var timeNow = DateTime.UtcNow;

            if (request.ProgramRequirements != null)
            {
                foreach (var req in request.ProgramRequirements)
                {
                    program.AcademicProgramRequirements.Add(new AcademicProgramRequirement
                    {
                        ProgramId = program.Id,
                        RequirementText = req,
                        CreatedAt = timeNow,
                        UpdatedAt = timeNow
                    });
                }
            }

            if (request.Notes != null)
            {
                foreach (var note in request.Notes)
                {
                    program.AcademicProgramNotes.Add(new AcademicProgramNote
                    {
                        ProgramId = program.Id,
                        NoteText = note,
                        CreatedAt = timeNow,
                        UpdatedAt = timeNow
                    });
                }
            }

            if (request.LecturingSystem != null)
            {
                foreach (var sys in request.LecturingSystem)
                {
                    program.AcademicProgramSystems.Add(new AcademicProgramSystem
                    {
                        ProgramId = program.Id,
                        SystemText = sys,
                        CreatedAt = timeNow,
                        UpdatedAt = timeNow
                    });
                }
            }

            if (request.LectureCategory != null)
            {
                foreach (var catDto in request.LectureCategory)
                {
                    var newCat = new AcademicCourseCategory
                    {
                        ProgramId = program.Id,
                        Name = catDto.CategoryName,
                        TotalCredits = catDto.TotalCredits ?? 0,
                        CreatedAt = timeNow,
                        UpdatedAt = timeNow
                    };

                    if (catDto.Lectures != null)
                    {
                        foreach (var lecDto in catDto.Lectures)
                        {
                            newCat.AcademicCourses.Add(new AcademicCourse
                            {
                                Name = lecDto.LectureName,
                                Credits = lecDto.Credits ?? 0,
                                Description = lecDto.Description,
                                CreatedAt = timeNow,
                                UpdatedAt = timeNow
                            });
                        }
                    }

                    program.AcademicCourseCategories.Add(newCat);
                }
            }

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Academic Program {Id} updated successfully.", program.Id);

            return new EditAcademicProgramResponse
            {
                Id = program.Id,
                ProgramName = program.Name,
                Slug = program.Slug,
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
                IsPublished = program.IsPublished,
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
        private string GenerateSlug(string phrase)
        {
            string str = phrase.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-");
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim('-');
            return str;
        }
    }
}
