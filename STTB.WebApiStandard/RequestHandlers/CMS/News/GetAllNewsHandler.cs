using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News
{
    public class GetAllNewsHandler : IRequestHandler<GetAllNewsRequest, GetAllNewsResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllNewsHandler> _logger;

        public GetAllNewsHandler(SttbDbContext db, ILogger<GetAllNewsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllNewsResponse> Handle(GetAllNewsRequest request, CancellationToken ct)
        {
            var query = _db.NewsPosts
                .Include(n => n.NewsPostCategories)
                    .ThenInclude(npc => npc.Category)
                .AsNoTracking();

            // Filter by Title
            if (!string.IsNullOrWhiteSpace(request.NewsName))
            {
                query = query.Where(n => n.Title.Contains(request.NewsName));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            // Fetch NewsPosts for this page
            var newsList = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(ct);

            // Fetch Assets to get image paths
            var newsIds = newsList.Select(n => n.Id).ToList();
            var assets = await _db.Assets
                .Where(a => a.ModelType == @"news_posts\news_image" && a.ModelId.HasValue && newsIds.Contains(a.ModelId.Value))
                .ToDictionaryAsync(a => a.ModelId.Value, a => a.FilePath, ct);

            var items = newsList.Select(n => new NewsDTO
            {
                Id = n.Id,
                Slug = n.Slug,
                Title = n.Title,
                Content = n.Content, // Map to pascal case contract property
                PublicationDate = n.PublishedAt,
                Category = n.NewsPostCategories.Select(npc => npc.Category.Name).ToList(),
                IsPublished = n.IsPublished,
                CreatedAt = n.CreatedAt,
                ImagePath = assets.TryGetValue(n.Id, out var path) ? path : string.Empty
            }).ToList();

            _logger.LogInformation("Found {Count} news posts out of {Total} total.", items.Count, totalItems);

            return new GetAllNewsResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalNews = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<NewsPost> ApplySorting(
            IQueryable<NewsPost> query,
            string orderBy,
            string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(n => n.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(n => n.Id) : query.OrderBy(n => n.Id),
                "Title" => isDescending ? query.OrderByDescending(n => n.Title) : query.OrderBy(n => n.Title),
                "PublicationDate" => isDescending ? query.OrderByDescending(n => n.PublishedAt) : query.OrderBy(n => n.PublishedAt),
                "CreatedAt" => isDescending ? query.OrderByDescending(n => n.CreatedAt) : query.OrderBy(n => n.CreatedAt),
                _ => query.OrderByDescending(n => n.CreatedAt)
            };
        }
    }
}
