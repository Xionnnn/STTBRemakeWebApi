using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Media
{
    public class GetAvailableVideoHandler : IRequestHandler<GetAvailableVideoRequest, GetAvailableVideoResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAvailableVideoHandler> _logger;

        public GetAvailableVideoHandler(SttbDbContext db, ILogger<GetAvailableVideoHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAvailableVideoResponse> Handle(GetAvailableVideoRequest request, CancellationToken ct)
        {
            // 1. Initial query: IsPublished and MediaFormat = 'Video'
            var query = _db.MediaItems
                .Include(m => m.MediaItemTopics)
                    .ThenInclude(mt => mt.TopicCategory)
                .Where(m => m.IsPublished && m.MediaFormat == "video")
                .AsNoTracking();

            // 2. Apply Filters
            if (!string.IsNullOrWhiteSpace(request.VideoTitle))
            {
                query = query.Where(m => m.Title.Contains(request.VideoTitle));
            }

            if (!string.IsNullOrWhiteSpace(request.AuthorName))
            {
                query = query.Where(m => m.AuthorName.Contains(request.AuthorName));
            }

            if (!string.IsNullOrWhiteSpace(request.CategoryName))
            {
                query = query.Where(m => m.MediaItemTopics
                    .Any(mt => mt.TopicCategory.Name.Contains(request.CategoryName)));
            }

            if (request.PublicationDate.HasValue)
            {
                var dateUtc = DateTime.SpecifyKind(request.PublicationDate.Value.Date, DateTimeKind.Utc);
                query = query.Where(m => m.PublishedAt >= dateUtc && m.PublishedAt < dateUtc.AddDays(1));
            }

            // 3. Optional Fetch Limit
            if (request.FetchLimit.HasValue)
            {
                query = query.Take(request.FetchLimit.Value);
            }

            // 4. Apply Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            var totalItems = await query.CountAsync(ct);

            // 5. Pagination and Projection (including Theme, Author, Description, Category)
            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(m => new VideoDTO
                {
                    Id = m.Id,
                    Slug = m.Slug,
                    VideoTitle = m.Title,
                    AuthorName = m.AuthorName ?? string.Empty,
                    VideoDescription = m.Description ?? string.Empty,
                    Theme = m.Theme ?? string.Empty,
                    PublicationDate = m.PublishedAt,
                    Category = m.MediaItemTopics
                        .Select(mt => mt.TopicCategory.Name)
                        .ToList(),
                    ThumbnailPath = _db.Assets
                        .Where(a => a.ModelType == "media_items\\video_thumbnail" && a.ModelId == m.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} videos out of {totalItems} total");

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            return new GetAvailableVideoResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalVideo = totalItems,  
                TotalPages = totalPages
            };
        }

        private IQueryable<MediaItem> ApplySorting(
            IQueryable<MediaItem> query,
            string orderBy,
            string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(m => m.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "VideoTitle" => isDescending
                    ? query.OrderByDescending(m => m.Title)
                    : query.OrderBy(m => m.Title),

                "AuthorName" => isDescending
                    ? query.OrderByDescending(m => m.AuthorName)
                    : query.OrderBy(m => m.AuthorName),

                // Default: latest created_at
                _ => query.OrderByDescending(m => m.CreatedAt)
            };
        }
    }
}
