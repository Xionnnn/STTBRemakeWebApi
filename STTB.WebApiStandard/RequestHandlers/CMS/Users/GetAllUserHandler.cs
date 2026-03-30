using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Users
{
    public class GetAllUserHandler : IRequestHandler<GetAllIUserRequest, GetAllUserResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllUserHandler> _logger;

        public GetAllUserHandler(SttbDbContext db, ILogger<GetAllUserHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllUserResponse> Handle(GetAllIUserRequest request, CancellationToken ct)
        {
            var query = _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .AsNoTracking();

            // Filter by name
            if (!string.IsNullOrWhiteSpace(request.UserName))
            {
                query = query.Where(u => u.FullName.Contains(request.UserName));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            // Count total before pagination
            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            // Paginate
            var userList = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(ct);

            var items = userList.Select(u => new CMSUserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                IsActive = u.IsActive,
                LastLoginAt = u.LastLoginAt ?? DateTime.MinValue,
                CreatedAt = u.CreatedAt,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
                Permissions = u.UserPermissions.Select(up => up.Permission.Name).ToList()
            }).ToList();

            _logger.LogInformation("Found {Count} users out of {Total} total.", items.Count, totalItems);

            return new GetAllUserResponse
            {
                Items = items
            };
        }

        private IQueryable<User> ApplySorting(IQueryable<User> query, string orderBy, string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(u => u.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending
                    ? query.OrderByDescending(u => u.Id)
                    : query.OrderBy(u => u.Id),

                "FullName" => isDescending
                    ? query.OrderByDescending(u => u.FullName)
                    : query.OrderBy(u => u.FullName),

                "Email" => isDescending
                    ? query.OrderByDescending(u => u.Email)
                    : query.OrderBy(u => u.Email),

                "IsActive" => isDescending
                    ? query.OrderByDescending(u => u.IsActive)
                    : query.OrderBy(u => u.IsActive),

                "LastLoginAt" => isDescending
                    ? query.OrderByDescending(u => u.LastLoginAt)
                    : query.OrderBy(u => u.LastLoginAt),

                "CreatedAt" => isDescending
                    ? query.OrderByDescending(u => u.CreatedAt)
                    : query.OrderBy(u => u.CreatedAt),

                _ => query.OrderByDescending(u => u.CreatedAt)
            };
        }
    }
}
