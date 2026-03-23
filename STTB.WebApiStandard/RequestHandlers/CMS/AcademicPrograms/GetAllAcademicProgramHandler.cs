using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms
{
    public class GetAllAcademicProgramHandler : IRequestHandler<GetAllAcademicProgramRequest, GetAllAcademicProgramResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllAcademicProgramHandler> _logger;

        public GetAllAcademicProgramHandler(SttbDbContext db, ILogger<GetAllAcademicProgramHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllAcademicProgramResponse> Handle(GetAllAcademicProgramRequest request, CancellationToken ct)
        {
            var query = _db.AcademicPrograms.AsNoTracking();

            // Filter by name
            if (!string.IsNullOrWhiteSpace(request.AcademicProgramName))
            {
                query = query.Where(a => a.Name.Contains(request.AcademicProgramName));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new AcademicProgramDTO
                {
                    Id = a.Id,
                    Slug = a.Slug,
                    ProgramName = a.Name,
                    Degree = a.DegreeAbbr,
                    Duration = a.StudyDuration,
                    TotalCredit = a.TotalCredits,
                    IsPublished = a.IsPublished ? "Yes" : "No",
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync(ct);

            _logger.LogInformation("Found {Count} academic programs out of {Total} total.", items.Count, totalItems);

            return new GetAllAcademicProgramResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<AcademicProgram> ApplySorting(
            IQueryable<AcademicProgram> query,
            string orderBy,
            string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(a => a.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id),
                "ProgramName" => isDescending ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
                "Degree" => isDescending ? query.OrderByDescending(a => a.DegreeAbbr) : query.OrderBy(a => a.DegreeAbbr),
                "Duration" => isDescending ? query.OrderByDescending(a => a.StudyDuration) : query.OrderBy(a => a.StudyDuration),
                "TotalCredit" => isDescending ? query.OrderByDescending(a => a.TotalCredits) : query.OrderBy(a => a.TotalCredits),
                "CreatedAt" => isDescending ? query.OrderByDescending(a => a.CreatedAt) : query.OrderBy(a => a.CreatedAt),
                _ => query.OrderByDescending(a => a.CreatedAt)
            };
        }
    }
}
