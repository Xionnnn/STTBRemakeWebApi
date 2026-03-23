using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events
{
    public class GetAllEventHandler : IRequestHandler<GetAllEventRequest, GetAllEventResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllEventHandler> _logger;

        public GetAllEventHandler(SttbDbContext db, ILogger<GetAllEventHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllEventResponse> Handle(GetAllEventRequest request, CancellationToken ct)
        {
            var query = _db.Events
                .Include(e => e.EventOrganizer)
                .Include(e => e.EventCategoryMaps)
                    .ThenInclude(m => m.Category)
                .AsNoTracking();

            // Filter by Title
            if (!string.IsNullOrWhiteSpace(request.EventName))
            {
                query = query.Where(e => e.Title.Contains(request.EventName));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            // Fetch image paths manually if they are stored in the Assets table
            var eventsList = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(ct);

            // Fetch Assets for the events in this page
            var eventIds = eventsList.Select(e => e.Id).ToList();
            var assets = await _db.Assets
                .Where(a => a.ModelType == @"events\event_image" && eventIds.Contains(a.ModelId))
                .ToDictionaryAsync(a => a.ModelId, a => a.FilePath, ct);

            var items = eventsList.Select(e => new EventDTO
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
                CreatedAt = e.CreatedAt,
                ImagePath = assets.TryGetValue(e.Id, out var path) ? path : string.Empty
            }).ToList();

            _logger.LogInformation("Found {Count} events out of {Total} total.", items.Count, totalItems);

            return new GetAllEventResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalEvents = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<Event> ApplySorting(
            IQueryable<Event> query,
            string orderBy,
            string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(e => e.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id),
                "EventTitle" => isDescending ? query.OrderByDescending(e => e.Title) : query.OrderBy(e => e.Title),
                "Location" => isDescending ? query.OrderByDescending(e => e.Location) : query.OrderBy(e => e.Location),
                "StartsAtDate" => isDescending ? query.OrderByDescending(e => e.StartAt) : query.OrderBy(e => e.StartAt),
                "EndsAtDate" => isDescending ? query.OrderByDescending(e => e.EndAt) : query.OrderBy(e => e.EndAt),
                "OrganizerName" => isDescending ? query.OrderByDescending(e => e.EventOrganizer!.Name) : query.OrderBy(e => e.EventOrganizer!.Name),
                "CreatedAt" => isDescending ? query.OrderByDescending(e => e.CreatedAt) : query.OrderBy(e => e.CreatedAt),
                _ => query.OrderByDescending(e => e.CreatedAt)
            };
        }
    }
}
