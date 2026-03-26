using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms
{
    public class AddAcademicProgramHandler : IRequestHandler<AddAcademicProgramRequest, AddAcademicProgramResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddAcademicProgramHandler> _logger;

        public AddAcademicProgramHandler(SttbDbContext db, ILogger<AddAcademicProgramHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddAcademicProgramResponse> Handle(AddAcademicProgramRequest request, CancellationToken ct)
        {
            var slug = GenerateSlug(request.Slug);

            var program = new AcademicProgram
            {
                Name = request.ProgramName,
                Slug = slug,
                GraduateProfileDescription = request.ProgramDescription,
                DegreeAbbr = request.Degree,
                GraduateProfileMotto = request.Motto,
                InformedDescription = request.InformedDescription,
                TransformedDescription = request.TransformedDescription,
                TransformativeDescription = request.TransformativeDescription,
                TotalCredits = request.TotalCredits ?? 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (request.Duration.HasValue)
            {
                program.StudyDuration = request.Duration.Value;
            }

            await _db.AcademicPrograms.AddAsync(program, ct);

            var timeNow = DateTime.UtcNow;

            // Rebuild collections
            if (request.ProgramRequirements != null)
            {
                foreach (var req in request.ProgramRequirements)
                {
                    program.AcademicProgramRequirements.Add(new AcademicProgramRequirement
                    {
                        Program = program,
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
                        Program = program,
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
                        Program = program,
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
                        Program = program,
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
            _logger.LogInformation("Academic Program {Id} created successfully.", program.Id);

            return new AddAcademicProgramResponse
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
