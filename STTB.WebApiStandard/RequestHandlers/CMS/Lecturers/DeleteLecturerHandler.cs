using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Lecturers
{
    public class DeleteLecturerHandler : IRequestHandler<DeleteLecturerRequest, DeleteLecturerResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteLecturerHandler> _logger;

        public DeleteLecturerHandler(SttbDbContext db, ILogger<DeleteLecturerHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteLecturerResponse> Handle(DeleteLecturerRequest request, CancellationToken ct)
        {
            var lecturer = await _db.Lecturers
                .Include(l => l.LecturerRoleMaps)
                .Include(l => l.LecturerDegreeMaps)
                .FirstOrDefaultAsync(l => l.Id == request.Id, ct);

            if (lecturer == null)
            {
                throw new KeyNotFoundException($"Lecturer with ID {request.Id} was not found.");
            }

            // Remove associated Role Maps
            if (lecturer.LecturerRoleMaps.Any())
            {
                _db.LecturerRoleMaps.RemoveRange(lecturer.LecturerRoleMaps);
            }

            // Remove associated Degree Maps
            if (lecturer.LecturerDegreeMaps.Any())
            {
                _db.LecturerDegreeMaps.RemoveRange(lecturer.LecturerDegreeMaps);
            }

            // Remove associated Image Asset
            var existingAsset = await _db.Assets
                .FirstOrDefaultAsync(a => a.ModelType == @"lecturers\lecturer_image" && a.ModelId == lecturer.Id, ct);

            if (existingAsset != null)
            {
                var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingAsset.FilePath.TrimStart('/'));
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                }
                _db.Assets.Remove(existingAsset);
            }

            // Remove Lecturer
            _db.Lecturers.Remove(lecturer);

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Lecturer {Id} deleted successfully.", lecturer.Id);

            return new DeleteLecturerResponse();
        }
    }
}
