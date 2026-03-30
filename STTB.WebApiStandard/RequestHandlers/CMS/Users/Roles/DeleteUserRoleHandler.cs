using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Roles;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Users.Roles
{
    public class DeleteUserRoleHandler : IRequestHandler<DeleteUserRoleRequest, DeleteUserRoleResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteUserRoleHandler> _logger;

        public DeleteUserRoleHandler(SttbDbContext db, ILogger<DeleteUserRoleHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteUserRoleResponse> Handle(DeleteUserRoleRequest request, CancellationToken ct)
        {
            var role = await _db.Roles
                .Include(r => r.UserRoles)
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.Id} was not found.");
            }

            // Remove related mappings
            _db.UserRoles.RemoveRange(role.UserRoles);
            _db.RolePermissions.RemoveRange(role.RolePermissions);
            _db.Roles.Remove(role);

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Role {Id} deleted successfully.", role.Id);

            return new DeleteUserRoleResponse();
        }
    }
}
