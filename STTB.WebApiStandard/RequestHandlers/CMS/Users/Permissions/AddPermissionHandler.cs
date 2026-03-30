using MediatR;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Users.Permissions
{
    public class AddPermissionHandler : IRequestHandler<AddPermissionRequest, AddPermissionResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddPermissionHandler> _logger;

        public AddPermissionHandler(SttbDbContext db, ILogger<AddPermissionHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddPermissionResponse> Handle(AddPermissionRequest request, CancellationToken ct)
        {
            var permission = new Permission
            {
                Name = request.PermissionName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Permissions.AddAsync(permission, ct);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Permission {Id} created successfully.", permission.Id);

            return new AddPermissionResponse
            {
                Id = permission.Id,
                PermissionName = permission.Name
            };
        }
    }
}
