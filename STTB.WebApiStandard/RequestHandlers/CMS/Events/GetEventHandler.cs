using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events
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
            var e = await _db.Events
                .Include(ev => ev.EventOrganizer)
                .Include(ev => ev.EventCategoryMaps)
                    .ThenInclude(m => m.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(ev => ev.Id == request.Id, ct);

            if (e == null)
            {
                throw new KeyNotFoundException($"Event with ID {request.Id} was not found.");
            }

            var asset = await _db.Assets
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ModelType == @"events\event_image" && a.ModelId == e.Id, ct);

            return new GetEventResponse
            {
                Id = e.Id,
                Slug = e.Slug,
                EventTitle = e.Title,
                Description = e.Description ?? string.Empty,
                Location = e.Location ?? string.Empty,
                StartsAtDate = e.StartAt,
                EndsAtDate = e.EndAt,
                OrganizerName = e.EventOrganizer?.Name ?? string.Empty,
                Category = e.EventCategoryMaps.Select(m => m.Category.Name).ToList(),
                IsPublished = e.IsPublished,
                ImagePath = asset?.FilePath ?? string.Empty
            };
        }
    }
}
