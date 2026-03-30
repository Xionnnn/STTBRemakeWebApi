using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Users.Permissions
{
    public class DeletePermissionHandler : IRequestHandler<DeletePermissionRequest, DeletePermissionResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeletePermissionHandler> _logger;

        public DeletePermissionHandler(SttbDbContext db, ILogger<DeletePermissionHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeletePermissionResponse> Handle(DeletePermissionRequest request, CancellationToken ct)
        {
            var permission = await _db.Permissions
                .Include(p => p.UserPermissions)
                .Include(p => p.RolePermissions)
                .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (permission == null)
            {
                throw new KeyNotFoundException($"Permission with ID {request.Id} was not found.");
            }

            // Remove related mappings
            _db.UserPermissions.RemoveRange(permission.UserPermissions);
            _db.RolePermissions.RemoveRange(permission.RolePermissions);
            _db.Permissions.Remove(permission);

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Permission {Id} deleted successfully.", permission.Id);

            return new DeletePermissionResponse();
        }
    }
}
