using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Profiles;
using STTB.WebApiStandard.Contracts.ResponseModels.Profiles;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Profiles
{
    public class GetAllAdministratorHandler : IRequestHandler<GetAllAdministratorrequest, GetAllAdministratorResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllAdministratorHandler> _logger;

        public GetAllAdministratorHandler(SttbDbContext db, ILogger<GetAllAdministratorHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllAdministratorResponse> Handle(GetAllAdministratorrequest request, CancellationToken ct)
        {
            var items = await _db.FoundationAdministrators
                .AsNoTracking()
                .OrderBy(a => a.AdminName)
                .Select(a => new AdministratorDTO
                {
                    Id = a.Id,
                    Name = a.AdminName,
                    Division = a.Division,
                    Role = a.Role ?? string.Empty
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} administrators");

            return new GetAllAdministratorResponse
            {
                Items = items
            };
        }
    }
}
