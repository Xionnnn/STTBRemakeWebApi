using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Lecturers;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Lecturers
{
    public class GetAllLecturerHandler : IRequestHandler<GetAllLecturerRequest, GetAllLecturerResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllLecturerHandler> _logger;

        public GetAllLecturerHandler(SttbDbContext db, ILogger<GetAllLecturerHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllLecturerResponse> Handle(GetAllLecturerRequest request, CancellationToken ct)
        {
            var query = _db.Lecturers
                .Include(l => l.LecturerRoleMaps)
                    .ThenInclude(lrm => lrm.LecturerRole)
                .Include(l => l.LecturerDegreeMaps)
                    .ThenInclude(ldm => ldm.LecturerDegree)
                .AsNoTracking();

            // Filter by name
            if (!string.IsNullOrWhiteSpace(request.LecturerName))
            {
                query = query.Where(l => l.LecturerName.Contains(request.LecturerName));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            // Count total before pagination
            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            // Paginate and project
            var lecturerList = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(ct);

            var lecturerIds = lecturerList.Select(l => l.Id).ToList();
            var imageAssets = await _db.Assets
                .Where(a => a.ModelId.HasValue && a.ModelType != null && lecturerIds.Contains(a.ModelId.Value) && a.ModelType == @"lecturers\lecturer_image")
                .ToListAsync(ct);

            var imageLookup = imageAssets
                .GroupBy(a => a.ModelId!.Value)
                .ToDictionary(g => g.Key, g => g.First().FilePath);

            var items = lecturerList.Select(l => new LecturerDTO
            {
                Id = l.Id,
                LecturerName = l.LecturerName,
                OrganizationalRole = l.OrganizationalRole,
                Roles = l.LecturerRoleMaps.Select(rm => rm.LecturerRole.RoleName).ToList(),
                Degrees = l.LecturerDegreeMaps.Select(dm => dm.LecturerDegree.DegreeName).ToList(),
                IsActive = l.IsActive,
                JoinedAt = l.JoinedAt,
                CreatedAt = l.CreatedAt,
                LecturerImagePath = imageLookup.ContainsKey(l.Id) ? imageLookup[l.Id] : string.Empty
            }).ToList();

            _logger.LogInformation("Found {Count} lecturers out of {Total} total.", items.Count, totalItems);

            return new GetAllLecturerResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<Lecturer> ApplySorting(
            IQueryable<Lecturer> query,
            string orderBy,
            string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(l => l.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending
                    ? query.OrderByDescending(l => l.Id)
                    : query.OrderBy(l => l.Id),

                "LecturerName" => isDescending
                    ? query.OrderByDescending(l => l.LecturerName)
                    : query.OrderBy(l => l.LecturerName),

                "OrganizationalRole" => isDescending
                    ? query.OrderByDescending(l => l.OrganizationalRole)
                    : query.OrderBy(l => l.OrganizationalRole),

                "IsActive" => isDescending
                    ? query.OrderByDescending(l => l.IsActive)
                    : query.OrderBy(l => l.IsActive),

                "JoinedAt" => isDescending
                    ? query.OrderByDescending(l => l.JoinedAt)
                    : query.OrderBy(l => l.JoinedAt),

                "CreatedAt" => isDescending
                    ? query.OrderByDescending(l => l.CreatedAt)
                    : query.OrderBy(l => l.CreatedAt),

                _ => query.OrderByDescending(l => l.CreatedAt)
            };
        }
    }
}
