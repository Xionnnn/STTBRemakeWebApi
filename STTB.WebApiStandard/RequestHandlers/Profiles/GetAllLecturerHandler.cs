using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Profiles;
using STTB.WebApiStandard.Contracts.ResponseModels.profiles;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Profiles
{
    public class GetAllLecturerHandler : IRequestHandler<GetAllLecturerRequest, GetAllLecturerResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllLecturerHandler> _logger;

        public GetAllLecturerHandler(SttbDbContext db, ILogger<GetAllLecturerHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllLecturerResponse> Handle(GetAllLecturerRequest request, CancellationToken ct)
        {
            var items = await _db.Lecturers
                .Include(l => l.LecturerRoleMaps)
                    .ThenInclude(lrm => lrm.LecturerRole)
                .Include(l => l.LecturerDegreeMaps)
                    .ThenInclude(ldm => ldm.LecturerDegree)
                .Where(l => l.IsActive)
                .AsNoTracking()
                .Select(l => new LecturerDto
                {
                    Id = l.Id,
                    OrganizationalRole = l.OrganizationalRole,
                    LecturerName = l.LecturerName,
                    LecturerImagePath = _db.Assets
                        .Where(a => a.ModelType == @"lecturers\lecturer_image" && a.ModelId == l.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty,
                    Roles = l.LecturerRoleMaps.Select(rm => rm.LecturerRole.RoleName).ToList(),
                    Degrees = l.LecturerDegreeMaps.Select(dm => dm.LecturerDegree.DegreeName).ToList()
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} active lecturers");

            return new GetAllLecturerResponse
            {
                Items = items
            };
        }
    }
}
