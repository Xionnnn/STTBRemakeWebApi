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
    public class AddUserRoleHandler : IRequestHandler<AddUserRoleRequest, AddUserRoleResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddUserRoleHandler> _logger;

        public AddUserRoleHandler(SttbDbContext db, ILogger<AddUserRoleHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddUserRoleResponse> Handle(AddUserRoleRequest request, CancellationToken ct)
        {
            var role = new Role
            {
                Name = request.RoleName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Roles.AddAsync(role, ct);
            await _db.SaveChangesAsync(ct);

            // Add role permissions
            if (request.RolePermissions != null && request.RolePermissions.Any())
            {
                foreach (var permName in request.RolePermissions)
                {
                    var permission = await _db.Permissions.FirstOrDefaultAsync(p => p.Name == permName, ct);
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

                await _db.SaveChangesAsync(ct);
            }

            _logger.LogInformation("Role {Id} created successfully.", role.Id);

            return new AddUserRoleResponse
            {
                Id = role.Id,
                RoleName = role.Name,
                RolePermissions = request.RolePermissions ?? new System.Collections.Generic.List<string>()
            };
        }
    }
}
