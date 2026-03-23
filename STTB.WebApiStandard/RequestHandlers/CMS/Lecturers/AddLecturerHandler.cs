using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Lecturers
{
    public class AddLecturerHandler : IRequestHandler<AddLecturerRequest, AddLecturerResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddLecturerHandler> _logger;

        public AddLecturerHandler(SttbDbContext db, ILogger<AddLecturerHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddLecturerResponse> Handle(AddLecturerRequest request, CancellationToken ct)
        {
            var lecturer = new Lecturer
            {
                LecturerName = request.LecturerName,
                OrganizationalRole = request.OrganizationalRole,
                IsActive = request.IsActive,
                JoinedAt = request.JoinedAt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Lecturers.AddAsync(lecturer, ct);

            // Handle Roles
            if (request.Roles != null && request.Roles.Any())
            {
                foreach (var roleName in request.Roles)
                {
                    var role = await _db.LecturerRoles.FirstOrDefaultAsync(r => r.RoleName == roleName, ct);
                    if (role == null)
                    {
                        throw new InvalidOperationException($"Lecturer Role '{roleName}' does not exist.");
                    }
                    lecturer.LecturerRoleMaps.Add(new LecturerRoleMap
                    {
                        Lecturer = lecturer,
                        LecturerRoleId = role.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            // Handle Degrees
            if (request.Degrees != null && request.Degrees.Any())
            {
                foreach (var degreeName in request.Degrees)
                {
                    var degree = await _db.LecturerDegrees.FirstOrDefaultAsync(d => d.DegreeName == degreeName, ct);
                    if (degree == null)
                    {
                        throw new InvalidOperationException($"Lecturer Degree '{degreeName}' does not exist.");
                    }
                    lecturer.LecturerDegreeMaps.Add(new LecturerDegreeMap
                    {
                        Lecturer = lecturer,
                        LecturerDegreeId = degree.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            await _db.SaveChangesAsync(ct); // Save Lecturer + Maps to get ID

            // Handle Image Upload
            string finalImagePath = string.Empty;
            if (request.LecturerImage != null && request.LecturerImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.LecturerImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.LecturerImage.CopyToAsync(fileStream, ct);
                }

                finalImagePath = $"/Uploads/images/{uniqueFileName}";

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

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Lecturer {Id} created successfully.", lecturer.Id);

            return new AddLecturerResponse
            {
                Id = lecturer.Id,
                LecturerName = lecturer.LecturerName,
                ImagePath = finalImagePath,
                OrganizationalRole = lecturer.OrganizationalRole,
                Roles = request.Roles ?? Array.Empty<string>(),
                Degrees = request.Degrees ?? Array.Empty<string>(),
                IsActive = lecturer.IsActive,
                JoinedAt = lecturer.JoinedAt
            };
        }
    }
}
