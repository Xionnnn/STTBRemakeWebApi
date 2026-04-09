using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.DTOs.CMS.AcademicCourses;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms.ProgramCourses
{
    public class GetAllAcademicProgramCoursesHandler : IRequestHandler<GetAllAcademicProgramCoursesRequest, GetAllAcademicProgramCoursesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllAcademicProgramCoursesHandler> _logger;

        public GetAllAcademicProgramCoursesHandler(SttbDbContext db, ILogger<GetAllAcademicProgramCoursesHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllAcademicProgramCoursesResponse> Handle(GetAllAcademicProgramCoursesRequest request, CancellationToken ct)
        {
            if (request.FetchAll)
            {
                var courses = await _db.AcademicProgramCourses
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .Select(c => new GetAllAcademicCourseDTO
                    {
                        Id = c.Id,
                        CourseName = c.Name,
                        Credits = c.Credits,
                        Description = c.Description ?? string.Empty
                    })
                    .ToListAsync(ct);

                var response = new GetAllAcademicProgramCoursesResponse { Items = courses };

                _logger.LogInformation("Retrieved all AcademicProgramCourses. Total items: {Count}", courses.Count);

                return response;
            }
            else
            {
                var query = _db.AcademicProgramCourses.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(request.CourseName))
                {
                    query = query.Where(c => c.Name.Contains(request.CourseName));
                }

                query = ApplySorting(query, request.OrderBy, request.OrderState);

                var totalItems = await query.CountAsync(ct);
                var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

                var courseList = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new GetAllAcademicCourseDTO
                    {
                        Id = c.Id,
                        CourseName = c.Name,
                        Credits = c.Credits,
                        Description = c.Description ?? string.Empty,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync(ct);

                var response = new GetAllAcademicProgramCoursesResponse
                {
                    Items = courseList,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    TotalItems = totalItems
                };

                _logger.LogInformation("Retrieved AcademicProgramCourses for page {PageNumber}. Items count: {Count}, Total Pages: {TotalPages}",
                    request.PageNumber, courseList.Count, totalPages);

                return response;
            }
        }

        private IQueryable<AcademicProgramCourse> ApplySorting(IQueryable<AcademicProgramCourse> query, string orderBy, string orderState)
        {
            var isDescending = orderState?.ToLower() == "desc";

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                "CreatedAt" => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                "CourseName" => isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                "Credits" => isDescending ? query.OrderByDescending(c => c.Credits) : query.OrderBy(c => c.Credits),
                "Description" => isDescending ? query.OrderByDescending(c => c.Description) : query.OrderBy(c => c.Description),
                _ => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
            };
        }
    }
}
