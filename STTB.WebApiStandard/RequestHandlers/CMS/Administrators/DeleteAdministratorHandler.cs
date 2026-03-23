using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Administrators
{
    public class DeleteAdministratorHandler : IRequestHandler<DeleteAdministratorRequest, DeleteAdministratorResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteAdministratorHandler> _logger;

        public DeleteAdministratorHandler(SttbDbContext db, ILogger<DeleteAdministratorHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteAdministratorResponse> Handle(DeleteAdministratorRequest request, CancellationToken ct)
        {
            var admin = await _db.FoundationAdministrators
                .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

            if (admin == null)
            {
                throw new KeyNotFoundException($"Administrator with ID {request.Id} was not found.");
            }

            _db.FoundationAdministrators.Remove(admin);
            
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Administrator {Id} deleted successfully.", admin.Id);

            return new DeleteAdministratorResponse();
        }
    }
}
