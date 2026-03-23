using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Lecturers
{
    public class GetLecturerHandler : IRequestHandler<GetLecturerRequest, GetLecturerResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetLecturerHandler> _logger;

        public GetLecturerHandler(SttbDbContext db, ILogger<GetLecturerHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetLecturerResponse> Handle(GetLecturerRequest request, CancellationToken ct)
        {
            var lecturer = await _db.Lecturers
                .Include(l => l.LecturerRoleMaps)
                    .ThenInclude(lrm => lrm.LecturerRole)
                .Include(l => l.LecturerDegreeMaps)
                    .ThenInclude(ldm => ldm.LecturerDegree)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == request.Id, ct);

            if (lecturer == null)
            {
                throw new KeyNotFoundException($"Lecturer with ID {request.Id} was not found.");
            }

            return new GetLecturerResponse
            {
                Id = lecturer.Id,
                LecturerName = lecturer.LecturerName,
                ImagePath = await _db.Assets
                    .Where(a => a.ModelType == @"lecturers\lecturer_image" && a.ModelId == lecturer.Id)
                    .Select(a => a.FilePath)
                    .FirstOrDefaultAsync(ct) ?? string.Empty,
                OrganizationalRole = lecturer.OrganizationalRole,
                Roles = lecturer.LecturerRoleMaps.Select(rm => rm.LecturerRole.RoleName).ToList(),
                Degrees = lecturer.LecturerDegreeMaps.Select(dm => dm.LecturerDegree.DegreeName).ToList(),
                IsActive = lecturer.IsActive,
                JoinedAt = lecturer.JoinedAt
            };
        }
    }
}
