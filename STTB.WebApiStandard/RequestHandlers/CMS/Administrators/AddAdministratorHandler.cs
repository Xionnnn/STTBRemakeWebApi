using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators;
using STTB.WebApiStandard.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Administrators
{
    public class AddAdministratorHandler : IRequestHandler<AddAdministratorRequest, AddAdministratorResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddAdministratorHandler> _logger;

        public AddAdministratorHandler(SttbDbContext db, ILogger<AddAdministratorHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddAdministratorResponse> Handle(AddAdministratorRequest request, CancellationToken ct)
        {
            var admin = new FoundationAdministrator
            {
                AdminName = request.Name,
                Division = request.Division,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.FoundationAdministrators.AddAsync(admin, ct);
            await _db.SaveChangesAsync(ct);
            
            _logger.LogInformation("Administrator {Id} created successfully.", admin.Id);

            return new AddAdministratorResponse
            {
                Id = admin.Id,
                Name = admin.AdminName,
                Division = admin.Division,
                Role = admin.Role ?? string.Empty
            };
        }
    }
}
