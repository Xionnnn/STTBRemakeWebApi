using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media
{
    public class GetAllMediaHandler : IRequestHandler<GetAllMediaRequest, GetAllMediaResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllMediaHandler> _logger;

        public GetAllMediaHandler(SttbDbContext db, ILogger<GetAllMediaHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllMediaResponse> Handle(GetAllMediaRequest request, CancellationToken ct)
        {
            var query = _db.MediaItems.AsNoTracking();

            // Filter by Name/Title
            if (!string.IsNullOrWhiteSpace(request.MediaName))
            {
                query = query.Where(m => m.Title.Contains(request.MediaName));
            }

            // Apply Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var mediaList = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(ct);

            var mediaIds = mediaList.Select(m => m.Id).ToList();
            var thumbnailAssets = await _db.Assets
                .Where(a => a.ModelId.HasValue && a.ModelType != null && mediaIds.Contains(a.ModelId.Value) && a.ModelType.Contains("thumbnail"))
                .ToListAsync(ct);

            var thumbnailLookup = thumbnailAssets
                .GroupBy(a => a.ModelId!.Value)
                .ToDictionary(g => g.Key, g => g.First().FilePath);

            var items = mediaList.Select(m => new MediaItemDTO
            {
                Id = m.Id,
                MediaName = m.Title,
                Slug = m.Slug,
                MediaFormat = m.MediaFormat,
                PublishedAt = m.PublishedAt,
                IsPublished = m.IsPublished,
                CreatedAt = m.CreatedAt,
                ThumbnailPath = thumbnailLookup.ContainsKey(m.Id) ? thumbnailLookup[m.Id] : string.Empty
            }).ToList();

            _logger.LogInformation("Found {Count} media items out of {Total} total.", items.Count, totalItems);

            return new GetAllMediaResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalMedia = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<MediaItem> ApplySorting(IQueryable<MediaItem> query, string orderBy, string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
                return query.OrderByDescending(m => m.CreatedAt);

            var isDesc = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "MediaName" => isDesc ? query.OrderByDescending(m => m.Title) : query.OrderBy(m => m.Title),
                "MediaFormat" => isDesc ? query.OrderByDescending(m => m.MediaFormat) : query.OrderBy(m => m.MediaFormat),
                "PublishedAt" => isDesc ? query.OrderByDescending(m => m.PublishedAt) : query.OrderBy(m => m.PublishedAt),
                "CreatedAt" => isDesc ? query.OrderByDescending(m => m.CreatedAt) : query.OrderBy(m => m.CreatedAt),
                _ => query.OrderByDescending(m => m.CreatedAt)
            };
        }
    }
}
