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
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, DeleteUserResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(SttbDbContext db, ILogger<DeleteUserHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserRequest request, CancellationToken ct)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .Include(u => u.UserPermissions)
                .FirstOrDefaultAsync(u => u.Id == request.Id, ct);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {request.Id} was not found.");
            }

            // Remove related data
            _db.UserRoles.RemoveRange(user.UserRoles);
            _db.UserPermissions.RemoveRange(user.UserPermissions);
            _db.Users.Remove(user);

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("User {Id} deleted successfully.", user.Id);

            return new DeleteUserResponse();
        }
    }
}
