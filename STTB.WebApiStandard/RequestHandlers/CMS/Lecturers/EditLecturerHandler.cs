using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Lecturers
{
    public class EditLecturerHandler : IRequestHandler<EditLecturerRequest, EditLecturerResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditLecturerHandler> _logger;

        public EditLecturerHandler(SttbDbContext db, ILogger<EditLecturerHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditLecturerResponse> Handle(EditLecturerRequest request, CancellationToken ct)
        {
            var lecturer = await _db.Lecturers
                .Include(l => l.LecturerRoleMaps)
                .Include(l => l.LecturerDegreeMaps)
                .FirstOrDefaultAsync(l => l.Id == request.Id, ct);

            if (lecturer == null)
            {
                throw new KeyNotFoundException($"Lecturer with ID {request.Id} was not found.");
            }

            // Update basic fields
            lecturer.LecturerName = request.LecturerName;
            lecturer.OrganizationalRole = request.OrganizationalRole;
            lecturer.IsActive = request.IsActive;
            lecturer.JoinedAt = DateTime.SpecifyKind(request.JoinedAt, DateTimeKind.Utc);
            lecturer.UpdatedAt = DateTime.UtcNow;

            // Handle Roles
            _db.LecturerRoleMaps.RemoveRange(lecturer.LecturerRoleMaps);
            foreach (var roleName in request.Roles)
            {
                var role = await _db.LecturerRoles.FirstOrDefaultAsync(r => r.RoleName == roleName, ct);
                if (role == null)
                {
                    role = new LecturerRole
                    {
                        RoleName = roleName,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _db.LecturerRoles.AddAsync(role, ct);
                }
                lecturer.LecturerRoleMaps.Add(new LecturerRoleMap
                {
                    Lecturer = lecturer,
                    LecturerRole = role,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // Handle Degrees
            _db.LecturerDegreeMaps.RemoveRange(lecturer.LecturerDegreeMaps);
            foreach (var degreeName in request.Degrees)
            {
                var degree = await _db.LecturerDegrees.FirstOrDefaultAsync(d => d.DegreeName == degreeName, ct);
                if (degree == null)
                {
                    degree = new LecturerDegree
                    {
                        DegreeName = degreeName,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _db.LecturerDegrees.AddAsync(degree, ct);
                }
                lecturer.LecturerDegreeMaps.Add(new LecturerDegreeMap
                {
                    Lecturer = lecturer,
                    LecturerDegree = degree,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // Handle Image Upload if provided
            string finalImagePath = string.Empty;
            if (request.LecturerImage != null && request.LecturerImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "images", "lecturers");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.LecturerImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.LecturerImage.CopyToAsync(fileStream, ct);
                }

                finalImagePath = $"/Uploads/images/lecturers/{uniqueFileName}";

                // Update Asset record
                var existingAsset = await _db.Assets
                    .FirstOrDefaultAsync(a => a.ModelType == @"lecturers\lecturer_image" && a.ModelId == lecturer.Id, ct);

                if (existingAsset != null)
                {
                    // Optional: delete old physical file
                    var oldPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingAsset.FilePath.TrimStart('/'));
                    if (File.Exists(oldPhysicalPath)) File.Delete(oldPhysicalPath);

                    existingAsset.FilePath = finalImagePath;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"lecturers\lecturer_image",
                        ModelId = lecturer.Id,
                        FileName = uniqueFileName,
                        FilePath = finalImagePath,
                        MimeType = request.LecturerImage.ContentType,
                        SizeBytes = request.LecturerImage.Length,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }, ct);
                }
            }
            else
            {
                // Retrieve existing image path if no new image uploaded
                finalImagePath = await _db.Assets
                    .Where(a => a.ModelType == @"lecturers\lecturer_image" && a.ModelId == lecturer.Id)
                    .Select(a => a.FilePath)
                    .FirstOrDefaultAsync(ct) ?? string.Empty;
            }

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Lecturer {Id} updated successfully.", lecturer.Id);

            return new EditLecturerResponse
            {
                Id = lecturer.Id,
                LecturerName = lecturer.LecturerName,
                ImagePath = finalImagePath,
                OrganizationalRole = lecturer.OrganizationalRole,
                Roles = request.Roles,
                Degrees = request.Degrees,
                IsActive = lecturer.IsActive,
                JoinedAt = lecturer.JoinedAt
            };
        }
    }
}
