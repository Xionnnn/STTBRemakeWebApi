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
    public class GetUserHandler : IRequestHandler<GetUserRequest, GetUserResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetUserHandler> _logger;

        public GetUserHandler(SttbDbContext db, ILogger<GetUserHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken ct)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.Id} was not found.");
            }

            _logger.LogInformation("Fetched user {Id}.", user.Id);

            return new GetUserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                Permissions = user.UserPermissions.Select(up => up.Permission.Name).ToList()
            };
        }
    }
}
