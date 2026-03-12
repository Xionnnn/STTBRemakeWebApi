using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.Events;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Events
{
    public class GetEventHandler : IRequestHandler<GetEventRequest, GetEventResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetEventHandler> _logger;
        public GetEventHandler(SttbDbContext db, ILogger<GetEventHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetEventResponse> Handle(GetEventRequest request, CancellationToken ct)
        {
            var eventItem = await _db.Events
                .Include(e => e.EventCategoryMaps)
                    .ThenInclude(ecm => ecm.Category)
                .Include(e => e.EventOrganizer)
                .Where(e => e.Slug == request.EventSlug && e.IsPublished)
                .AsNoTracking()
                .Select(e => new GetEventResponse
                {
                    EventName = e.Title,
                    StartAtDate = e.StartAt,
                    EndsAtDate = e.EndAt,
                    Location = e.Location ?? string.Empty,
                    OrganizerName = e.EventOrganizer != null ? e.EventOrganizer.Name : string.Empty,
                    Category = e.EventCategoryMaps
                        .Select(ecm => ecm.Category.Name)
                        .FirstOrDefault() ?? string.Empty,
                    Description = e.Description ?? string.Empty,
                    ImagePath = _db.Assets
                        .Where(a => a.ModelType == "events\\event_image" && a.ModelId == e.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .FirstOrDefaultAsync(ct);

            if (eventItem == null)
            {
                _logger.LogInformation($"Event with slug '{request.EventSlug}' not found");
            }

            return eventItem;
        }
    }
}
