using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Roles;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Users.Roles
{
    public class GetUserRoleHandler : IRequestHandler<GetUserRoleRequest, GetUserRoleResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetUserRoleHandler> _logger;

        public GetUserRoleHandler(SttbDbContext db, ILogger<GetUserRoleHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetUserRoleResponse> Handle(GetUserRoleRequest request, CancellationToken ct)
        {
            var role = await _db.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {request.Id} was not found.");
            }

            return new GetUserRoleResponse
            {
                Id = role.Id,
                RoleName = role.Name,
                RolePermissions = role.RolePermissions.Select(rp => rp.Permission.Name).ToList()
            };
        }
    }
}
