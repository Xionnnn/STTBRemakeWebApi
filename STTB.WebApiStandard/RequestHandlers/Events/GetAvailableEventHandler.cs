using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.Events;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Events
{
    public class GetAvailableEventsHandler : IRequestHandler<GetAvailableEventRequest, GetAvailableEventResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAvailableEventsHandler> _logger;
        public GetAvailableEventsHandler(SttbDbContext db, ILogger<GetAvailableEventsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAvailableEventResponse> Handle(GetAvailableEventRequest request, CancellationToken ct)
        {
            var query = _db.Events
                .Include(e => e.EventCategoryMaps)
                    .ThenInclude(ecm => ecm.Category)
                .Include(e => e.EventOrganizer)
                .Where(e => e.IsPublished)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.EventTitle))
            {
                query = query.Where(e => e.Title.Contains(request.EventTitle));
            }

            if (!string.IsNullOrWhiteSpace(request.CategoryName))
            {
                query = query.Where(e => e.EventCategoryMaps
                    .Any(ecm => ecm.Category.Name.Contains(request.CategoryName)));
            }

            if (!string.IsNullOrWhiteSpace(request.OrganizerName))
            {
                query = query.Where(e => e.EventOrganizer != null
                    && e.EventOrganizer.Name.Contains(request.OrganizerName));
            }

            if (request.EventDate.HasValue)
            {
                var dateUtc = DateTime.SpecifyKind(request.EventDate.Value.Date, DateTimeKind.Utc);
                var nextDay = dateUtc.AddDays(1);

                query = query.Where(e =>
                    e.StartAt < nextDay &&
                    (e.EndAt == null || e.EndAt >= dateUtc));
            }

            if (request.FetchLimit.HasValue)
            {
                query = query.Take(request.FetchLimit.Value);
            }

            query = ApplySorting(query, request.OrderBy, request.OrderState);

            var totalItems = await query.CountAsync(ct);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new EventDTO
                {
                    EventTitle = e.Title,
                    Slug = e.Slug,
                    StartsAtDate = e.StartAt,
                    EndsAtDate = e.EndAt ?? default,
                    Description = e.Description ?? string.Empty,
                    OrganizerName = e.EventOrganizer != null ? e.EventOrganizer.Name : string.Empty,
                    Category = e.EventCategoryMaps
                        .Select(ecm => ecm.Category.Name)
                        .ToList(),
                    ImagePath = _db.Assets
                        .Where(a => a.ModelType == "events\\event_image" && a.ModelId == e.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} events out of {totalItems} total");

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            return new GetAvailableEventResponse
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
                "EventTitle" => isDescending
                    ? query.OrderByDescending(e => e.Title)
                    : query.OrderBy(e => e.Title),

                "CategoryName" => isDescending
                    ? query.OrderByDescending(e => e.EventCategoryMaps
                        .Select(ecm => ecm.Category.Name).FirstOrDefault())
                    : query.OrderBy(e => e.EventCategoryMaps
                        .Select(ecm => ecm.Category.Name).FirstOrDefault()),

                // Default: latest created_at
                _ => query.OrderByDescending(e => e.CreatedAt)
            };
        }
    }
}
