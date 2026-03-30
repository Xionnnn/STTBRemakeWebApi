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
    public class EditUserHandler : IRequestHandler<EditUserRequest, EditUserResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditUserHandler> _logger;

        public EditUserHandler(SttbDbContext db, ILogger<EditUserHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditUserResponse> Handle(EditUserRequest request, CancellationToken ct)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .Include(u => u.UserPermissions)
                .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.Id} was not found.");
            }

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            }

            _db.UserRoles.RemoveRange(user.UserRoles);
            if (request.Roles != null)
            {
                foreach (var roleName in request.Roles)
                {
                    var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
                    if (role != null)
                    {
                        user.UserRoles.Add(new UserRole
                        {
                            UserId = user.Id,
                            RoleId = role.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            _db.UserPermissions.RemoveRange(user.UserPermissions);
            if (request.Permissions != null)
            {
                foreach (var permName in request.Permissions)
                {
                    var permission = await _db.Permissions.FirstOrDefaultAsync(p => p.Name == permName, ct);
                    if (permission != null)
                    {
                        user.UserPermissions.Add(new UserPermission
                        {
                            UserId = user.Id,
                            PermissionId = permission.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("User {Id} updated successfully.", user.Id);

            return new EditUserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = request.Roles ?? new List<string>(),
                Permissions = request.Permissions ?? new List<string>()
            };
        }
    }
}
