using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Administrators
{
    public class GetAdministratorHandler : IRequestHandler<GetAdministratorRequest, GetAdministratorResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAdministratorHandler> _logger;

        public GetAdministratorHandler(SttbDbContext db, ILogger<GetAdministratorHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAdministratorResponse> Handle(GetAdministratorRequest request, CancellationToken ct)
        {
            var admin = await _db.FoundationAdministrators
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

            if (admin == null)
            {
                throw new KeyNotFoundException($"Administrator with ID {request.Id} was not found.");
            }

            return new GetAdministratorResponse
            {
                Id = admin.Id,
                Name = admin.AdminName,
                Division = admin.Division,
                Role = admin.Role ?? string.Empty
            };
        }
    }
}
