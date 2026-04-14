using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Administrators;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Administrators
{
    public class GetAllAdministratorHandler : IRequestHandler<GetAllAdministratorRequest, GetAllAdministratorResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllAdministratorHandler> _logger;

        public GetAllAdministratorHandler(SttbDbContext db, ILogger<GetAllAdministratorHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllAdministratorResponse> Handle(GetAllAdministratorRequest request, CancellationToken ct)
        {
            var query = _db.FoundationAdministrators.AsNoTracking();

            // Filter by name
            if (!string.IsNullOrWhiteSpace(request.AdministratorName))
            {
                query = query.Where(a => a.AdminName.ToLower().Contains(request.AdministratorName.ToLower()));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            // Count total before pagination
            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            // Paginate and project
            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new AdministratorDTO
                {
                    Id = a.Id,
                    Name = a.AdminName,
                    Division = a.Division,
                    Role = a.Role ?? string.Empty,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync(ct);

            _logger.LogInformation("Found {Count} administrators out of {Total} total.", items.Count, totalItems);

            return new GetAllAdministratorResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<FoundationAdministrator> ApplySorting(
            IQueryable<FoundationAdministrator> query,
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
                "Id" => isDescending
                    ? query.OrderByDescending(a => a.Id)
                    : query.OrderBy(a => a.Id),

                "Name" => isDescending
                    ? query.OrderByDescending(a => a.AdminName)
                    : query.OrderBy(a => a.AdminName),

                "Division" => isDescending
                    ? query.OrderByDescending(a => a.Division)
                    : query.OrderBy(a => a.Division),

                "Role" => isDescending
                    ? query.OrderByDescending(a => a.Role)
                    : query.OrderBy(a => a.Role),

                "CreatedAt" => isDescending
                    ? query.OrderByDescending(a => a.CreatedAt)
                    : query.OrderBy(a => a.CreatedAt),

                _ => query.OrderByDescending(a => a.CreatedAt)
            };
        }
    }
}
