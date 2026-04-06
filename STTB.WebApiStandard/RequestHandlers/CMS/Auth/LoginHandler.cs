using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Auth;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Auth;
using STTB.WebApiStandard.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Auth
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly SttbDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(SttbDbContext db, IConfiguration configuration, ILogger<LoginHandler> logger)
        {
            _db = db;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken ct)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == request.Email, ct);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Your account is inactive. Please contact an administrator.");
            }

            // Collect role names
            var roleNames = user.UserRoles
                .Select(ur => ur.Role.Name)
                .ToList();

            // Collect permissions from roles + direct user permissions (deduplicated)
            var rolePermissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name);

            var userPermissions = user.UserPermissions
                .Select(up => up.Permission.Name);

            var allPermissions = rolePermissions
                .Union(userPermissions)
                .Distinct()
                .ToList();

            // Build claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            foreach (var permission in allPermissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            // Generate JWT token
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"]!);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Update LastLoginAt
            var userToUpdate = await _db.Users.FindAsync(new object[] { user.Id }, ct);
            if (userToUpdate != null)
            {
                userToUpdate.LastLoginAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);
            }

            _logger.LogInformation("User {Email} logged in successfully.", user.Email);

            return new LoginResponse
            {
                Token = tokenString,
                FullName = user.FullName,
                Permissions = allPermissions,
                Email = user.Email,
                Roles = roleNames
            };
        }
    }
}
