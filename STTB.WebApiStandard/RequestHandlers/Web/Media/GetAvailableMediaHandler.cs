using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using STTB.WebApiStandard.Contracts.DTOs.Web.Media;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Web.Media
{
    public class GetAvailableMediaHandler : IRequestHandler<GetAvailableMediaRequest, GetAvailableMediaResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAvailableMediaHandler> _logger;

        public GetAvailableMediaHandler(SttbDbContext db, ILogger<GetAvailableMediaHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAvailableMediaResponse> Handle(GetAvailableMediaRequest request, CancellationToken ct)
        {
            var formatLower = request.MediaFormat.ToLower();
            var query = _db.MediaItems
                .Include(m => m.MediaItemTopics)
                    .ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters)
                    .ThenInclude(mw => mw.MediaWriter)
                .Where(m => m.IsPublished && m.MediaFormat.ToLower() == formatLower)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.MediaTitle))
            {
                query = query.Where(m => m.Title.ToLower().Contains(request.MediaTitle.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.AuthorName))
            {
                query = query.Where(m => m.MediaItemWriters
                    .Any(mw => mw.MediaWriter.AuthorName.ToLower().Contains(request.AuthorName.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(request.CategoryName))
            {
                query = query.Where(m => m.MediaItemTopics
                    .Any(mt => mt.TopicCategory.Name.ToLower().Contains(request.CategoryName.ToLower())));
            }

            if (request.PublicationDate.HasValue)
            {
                var dateUtc = DateTime.SpecifyKind(request.PublicationDate.Value.Date, DateTimeKind.Utc);
                query = query.Where(m => m.PublishedAt >= dateUtc && m.PublishedAt < dateUtc.AddDays(1));
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
                .Select(m => new MediaDTO
                {
                    Id = m.Id,
                    Slug = m.Slug,
                    MediaTitle = m.Title,
                    Authors = m.MediaItemWriters
                        .Select(mw => new AuthorDTO
                        {
                            AuthorName = mw.MediaWriter.AuthorName,
                            AuthorPosition = mw.MediaWriter.AuthorPosition ?? string.Empty
                        })
                        .ToList(),
                    MediaDescription = m.Description ?? string.Empty,
                    PublicationDate = m.PublishedAt,
                    Category = m.MediaItemTopics
                        .Select(mt => mt.TopicCategory.Name)
                        .ToList(),
                    ThumbnailPath = _db.Assets
                        .Where(a => a.ModelType == $"media_items\\{formatLower}_thumbnail" && a.ModelId == m.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} videos out of {totalItems} total");

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            return new GetAvailableMediaResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalMedia = totalItems,  
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
                "MediaTitle" => isDescending
                    ? query.OrderByDescending(m => m.Title)
                    : query.OrderBy(m => m.Title),

                "AuthorName" => isDescending
                    ? query.OrderByDescending(m => m.MediaItemWriters
                        .Select(mw => mw.MediaWriter.AuthorName)
                        .FirstOrDefault())
                    : query.OrderBy(m => m.MediaItemWriters
                        .Select(mw => mw.MediaWriter.AuthorName)
                        .FirstOrDefault()),

                // Default: latest created_at
                _ => query.OrderByDescending(m => m.CreatedAt)
            };
        }
    }
}
