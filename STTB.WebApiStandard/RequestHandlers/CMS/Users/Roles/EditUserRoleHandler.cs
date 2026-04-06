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
    public class EditUserRoleHandler : IRequestHandler<EditUserRoleRequest, EditUserRoleResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditUserRoleHandler> _logger;

        public EditUserRoleHandler(SttbDbContext db, ILogger<EditUserRoleHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditUserRoleResponse> Handle(EditUserRoleRequest request, CancellationToken ct)
        {
            var role = await _db.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.Id} was not found.");
            }

            // Update name
            role.Name = request.RoleName;
            role.UpdatedAt = DateTime.UtcNow;

            // Remove old permissions and re-add
            _db.RolePermissions.RemoveRange(role.RolePermissions);

            if (request.RolePermissions != null && request.RolePermissions.Any())
            {
                foreach (var permName in request.RolePermissions)
                {
                    var permission = await _db.Permissions
                        .FirstOrDefaultAsync(p => p.Name == permName, ct);

                    if (permission != null)
                    {
                        await _db.RolePermissions.AddAsync(new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permission.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }, ct);
                    }
                }
            }

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Role {Id} updated successfully.", role.Id);

            // Re-fetch to get updated permission names
            var updatedPermissions = await _db.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync(ct);

            return new EditUserRoleResponse
            {
                Id = role.Id,
                RoleName = role.Name,
                RolePermissions = updatedPermissions
            };
        }
    }
}
