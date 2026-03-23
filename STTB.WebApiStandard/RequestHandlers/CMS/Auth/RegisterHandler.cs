using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Auth;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Auth;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Auth
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<RegisterHandler> _logger;

        public RegisterHandler(SttbDbContext db, ILogger<RegisterHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken ct)
        {
            // Check if email already exists
            var existingUser = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == request.Email, ct);

            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            // Validate that the role exists
            var role = await _db.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == request.RoleName, ct);

            if (role == null)
            {
                throw new InvalidOperationException($"Role '{request.RoleName}' does not exist.");
            }

            // Create user
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Users.AddAsync(user, ct);
            await _db.SaveChangesAsync(ct);

            // Assign role to user
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.UserRoles.AddAsync(userRole, ct);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("User {Email} registered successfully with role {Role}.", user.Email, request.RoleName);

            return new RegisterResponse
            {
                FullName = user.FullName,
                Email = user.Email,
                RoleName = request.RoleName,
                IsSuccess = true
            };
        }
    }
}
