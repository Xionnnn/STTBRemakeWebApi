using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events
{
    public class AddEventHandler : IRequestHandler<AddEventRequest, AddEventResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddEventHandler> _logger;

        public AddEventHandler(SttbDbContext db, ILogger<AddEventHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddEventResponse> Handle(AddEventRequest request, CancellationToken ct)
        {
            var ev = new Event
            {
                Slug = request.Slug,
                Title = request.EventTitle,
                Description = request.Description,
                Location = request.Location,
                StartAt = DateTime.SpecifyKind(request.StartsAtDate, DateTimeKind.Utc),
                EndAt = request.EndsAtDate.HasValue ? DateTime.SpecifyKind(request.EndsAtDate.Value, DateTimeKind.Utc) : (DateTime?)null,
                IsPublished = request.IsPublished,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Handle Organizer
            if (!string.IsNullOrWhiteSpace(request.OrganizerName))
            {
                var organizer = await _db.EventOrganizers
                    .FirstOrDefaultAsync(o => o.Name.ToLower() == request.OrganizerName.ToLower(), ct);

                if (organizer == null)
                {
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

            await _db.Events.AddAsync(ev, ct);

            // Handle Categories
            if (request.Category != null && request.Category.Any())
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
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _db.EventCategories.AddAsync(cat, ct);
                        await _db.SaveChangesAsync(ct);
                    }

                    ev.EventCategoryMaps.Add(new EventCategoryMap
                    {
                        Event = ev,
                        CategoryId = cat.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            await _db.SaveChangesAsync(ct); // Save event and maps to get ID

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

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Event {Id} created successfully.", ev.Id);

            return new AddEventResponse
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
