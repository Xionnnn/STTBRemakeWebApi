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
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.Id} was not found.");
            }

            role.Name = request.RoleName;
            role.UpdatedAt = DateTime.UtcNow;


            //update permissions
            var oldPermissions = role.RolePermissions
                .Select(rp => rp.PermissionId)
                .ToHashSet();

            var newPermissions = (await _db.Permissions
                .Where(p => request.RolePermissions.Contains(p.Name))
                .Select(p => p.Id)
                .ToListAsync(ct))
                .ToHashSet();

            var toRemove = role.RolePermissions
                .Where(rp => !newPermissions.Contains(rp.PermissionId))
                .ToList();
            

            var toAdd = newPermissions
                .Where(np => !oldPermissions.Contains(np))
                .Select(np => new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = np,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                })
                .ToList();

            _db.RolePermissions.RemoveRange(toRemove);
            await _db.RolePermissions.AddRangeAsync(toAdd, ct);
            await _db.SaveChangesAsync(ct);

            var updatedPermissions = await _db.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync(ct);

            return new EditUserRoleResponse
            {
                Id = role.Id,
                RoleName = request.RoleName,
                RolePermissions = updatedPermissions
            };
        }
    }
}
