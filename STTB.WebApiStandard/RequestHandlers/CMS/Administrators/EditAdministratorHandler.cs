using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Administrators
{
    public class EditAdministratorHandler : IRequestHandler<EditAdministratorRequest, EditAdministratorResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditAdministratorHandler> _logger;

        public EditAdministratorHandler(SttbDbContext db, ILogger<EditAdministratorHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditAdministratorResponse> Handle(EditAdministratorRequest request, CancellationToken ct)
        {
            var admin = await _db.FoundationAdministrators
                .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

            if (admin == null)
            {
                throw new KeyNotFoundException($"Administrator with ID {request.Id} was not found.");
            }

            admin.AdminName = request.Name;
            admin.Division = request.Division;
            admin.Role = request.Role;
            admin.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Administrator {Id} updated successfully.", admin.Id);

            return new EditAdministratorResponse
            {
                Id = admin.Id,
                Name = admin.AdminName,
                Division = admin.Division,
                Role = admin.Role ?? string.Empty
            };
        }
    }
}
