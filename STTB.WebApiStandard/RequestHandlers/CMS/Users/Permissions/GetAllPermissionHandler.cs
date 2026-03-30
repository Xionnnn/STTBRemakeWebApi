using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Users.Permissions
{
    public class GetAllPermissionHandler : IRequestHandler<GetAllPermissionRequest, GetAllPermissionResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllPermissionHandler> _logger;

        public GetAllPermissionHandler(SttbDbContext db, ILogger<GetAllPermissionHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllPermissionResponse> Handle(GetAllPermissionRequest request, CancellationToken ct)
        {
            var query = _db.Permissions.AsNoTracking();

            // Filter by name
            if (!string.IsNullOrWhiteSpace(request.PermissionName))
            {
                query = query.Where(p => p.Name.Contains(request.PermissionName));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            if (request.FetchAll)
            {
                var allPermissions = await query
                    .Select(p => new PermissionDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CreatedAt = p.CreatedAt
                    })
                    .ToListAsync(ct);

                _logger.LogInformation("Fetched all {Count} permissions.", allPermissions.Count);

                return new GetAllPermissionResponse
                {
                    Items = allPermissions,
                    PageNumber = 1,
                    PageSize = allPermissions.Count,
                    TotalItems = allPermissions.Count,
                    TotalPages = 1
                };
            }

            // Paginated mode
            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new PermissionDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync(ct);

            _logger.LogInformation("Found {Count} permissions out of {Total} total.", items.Count, totalItems);

            return new GetAllPermissionResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<Permission> ApplySorting(IQueryable<Permission> query, string orderBy, string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(p => p.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
                "Name" => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "CreatedAt" => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };
        }
    }
}
