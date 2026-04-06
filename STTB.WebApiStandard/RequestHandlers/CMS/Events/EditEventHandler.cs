using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events
{
    public class EditEventHandler : IRequestHandler<EditEventRequest, EditEventResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditEventHandler> _logger;

        public EditEventHandler(SttbDbContext db, ILogger<EditEventHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditEventResponse> Handle(EditEventRequest request, CancellationToken ct)
        {
            var ev = await _db.Events
                .Include(e => e.EventOrganizer)
                .Include(e => e.EventCategoryMaps)
                .FirstOrDefaultAsync(e => e.Id == request.Id, ct);

            if (ev == null)
            {
                throw new KeyNotFoundException($"Event with ID {request.Id} was not found.");
            }

            // Update basic fields
            ev.Slug = request.Slug;
            ev.Title = request.EventTitle;
            ev.Description = request.Description;
            ev.Location = request.Location;
            ev.StartAt = request.StartsAtDate;
            ev.EndAt = request.EndsAtDate;
            ev.IsPublished = request.IsPublished;
            ev.UpdatedAt = DateTime.UtcNow;

            // Handle Organizer
            if (!string.IsNullOrWhiteSpace(request.OrganizerName))
            {
                var organizer = await _db.EventOrganizers
                    .FirstOrDefaultAsync(o => o.Name.ToLower() == request.OrganizerName.ToLower(), ct);

                if (organizer == null)
                {
                    // Create new Organizer as requested if missing
                    organizer = new EventOrganizer
                    {
                        Name = request.OrganizerName,
                        OrganizerType = "General", // Default type
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _db.EventOrganizers.AddAsync(organizer, ct);
                    await _db.SaveChangesAsync(ct);
                }

                ev.EventOrganizerId = organizer.Id;
            }
            else
            {
                ev.EventOrganizerId = null;
            }

            // Handle Categories
            _db.EventCategoryMaps.RemoveRange(ev.EventCategoryMaps);
            
            if (request.Category != null)
            {
                foreach (var categoryName in request.Category)
                {
                    if (string.IsNullOrWhiteSpace(categoryName)) continue;

                    var cat = await _db.EventCategories
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower(), ct);

                    if (cat == null)
                    {
                        cat = new EventCategory
                        {
                            Name = categoryName,
                            Slug = categoryName.ToLower().Replace(" ", "-"),
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _db.EventCategories.AddAsync(cat, ct);
                        await _db.SaveChangesAsync(ct); // Save to generate ID
                    }

                    ev.EventCategoryMaps.Add(new EventCategoryMap
                    {
                        EventId = ev.Id,
                        CategoryId = cat.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            // Handle Image Upload
            string finalImagePath = string.Empty;
            if (request.EventImage != null && request.EventImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "images", "events");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.EventImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.EventImage.CopyToAsync(fileStream, ct);
                }

                finalImagePath = $"/Uploads/images/events/{uniqueFileName}";

                var existingAsset = await _db.Assets
                    .FirstOrDefaultAsync(a => a.ModelType == @"events\event_image" && a.ModelId == ev.Id, ct);

                if (existingAsset != null)
                {
                    var oldPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingAsset.FilePath.TrimStart('/'));
                    if (File.Exists(oldPhysicalPath)) File.Delete(oldPhysicalPath);

                    existingAsset.FilePath = finalImagePath;
                    existingAsset.FileName = uniqueFileName;
                    existingAsset.MimeType = request.EventImage.ContentType;
                    existingAsset.SizeBytes = request.EventImage.Length;
                    existingAsset.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"events\event_image",
                        ModelId = ev.Id,
                        FileName = uniqueFileName,
                        FilePath = finalImagePath,
                        MimeType = request.EventImage.ContentType,
                        SizeBytes = request.EventImage.Length,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }, ct);
                }
            }
            else
            {
                finalImagePath = await _db.Assets
                    .Where(a => a.ModelType == @"events\event_image" && a.ModelId == ev.Id)
                    .Select(a => a.FilePath)
                    .FirstOrDefaultAsync(ct) ?? string.Empty;
            }

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Event {Id} updated successfully.", ev.Id);

            return new EditEventResponse
            {
                Id = ev.Id,
                Slug = ev.Slug,
                EventTitle = ev.Title,
                Description = ev.Description ?? string.Empty,
                Location = ev.Location ?? string.Empty,
                StartsAtDate = ev.StartAt,
                EndsAtDate = ev.EndAt,
                OrganizerName = request.OrganizerName,
                Category = request.Category ?? new List<string>(),
                ImagePath = finalImagePath,
                IsPublished = ev.IsPublished
            };
        }
    }
}
