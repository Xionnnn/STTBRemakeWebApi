using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Roles;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Users.Roles
{
    public class GetAllUserRoleHandler : IRequestHandler<GetAllUserRoleRequest, GetAllUserRoleResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllUserRoleHandler> _logger;

        public GetAllUserRoleHandler(SttbDbContext db, ILogger<GetAllUserRoleHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllUserRoleResponse> Handle(GetAllUserRoleRequest request, CancellationToken ct)
        {
            var query = _db.Roles.AsNoTracking();

            // Filter by name
            if (!string.IsNullOrWhiteSpace(request.RoleName))
            {
                query = query.Where(r => r.Name.Contains(request.RoleName));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            if (request.FetchAll)
            {
                var allRoles = await query
                    .Select(r => new RoleDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        CreatedAt = r.CreatedAt
                    })
                    .ToListAsync(ct);

                _logger.LogInformation("Fetched all {Count} roles.", allRoles.Count);

                return new GetAllUserRoleResponse
                {
                    Items = allRoles,
                    PageNumber = 1,
                    PageSize = allRoles.Count,
                    TotalItems = allRoles.Count,
                    TotalPages = 1
                };
            }

            // Paginated mode
            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(r => new RoleDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(ct);

            _logger.LogInformation("Found {Count} roles out of {Total} total.", items.Count, totalItems);

            return new GetAllUserRoleResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<Role> ApplySorting(IQueryable<Role> query, string orderBy, string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(r => r.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id),
                "Name" => isDescending ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
                "CreatedAt" => isDescending ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
                _ => query.OrderByDescending(r => r.CreatedAt)
            };
        }
    }
}
