using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.News;
using STTB.WebApiStandard.Contracts.ResponseModels.News;
using STTB.WebApiStandard.Contracts.DTOs.Web.News;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Web.News
{
    public class GetAvailableNewsHandler : IRequestHandler<GetAvailableNewsRequest, GetAvailableNewsResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAvailableNewsHandler> _logger;
        public GetAvailableNewsHandler(SttbDbContext db, ILogger<GetAvailableNewsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAvailableNewsResponse> Handle(GetAvailableNewsRequest request, CancellationToken ct)
        {
            var query = _db.NewsPosts
                .Include(n => n.NewsPostCategories)
                    .ThenInclude(npc => npc.Category)
                .Where(n => n.IsPublished)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.NewsTitle))
            {
                query = query.Where(n => n.Title.Contains(request.NewsTitle));
            }

            if (!string.IsNullOrWhiteSpace(request.CategoryName))
            {
                query = query.Where(n => n.NewsPostCategories
                    .Any(npc => npc.Category.Name.Contains(request.CategoryName)));
            }

            if (request.EventDate.HasValue)
            {
                var dateUtc = DateTime.SpecifyKind(request.EventDate.Value.Date, DateTimeKind.Utc);
                query = query.Where(n => n.PublishedAt >= dateUtc && n.PublishedAt < dateUtc.AddDays(1));
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
                .Select(n => new NewsDTO
                {
                    Id = n.Id,
                    Slug = n.Slug,
                    Title = n.Title,
                    content = n.Content,
                    PublicationDate = n.PublishedAt,
                    Category = n.NewsPostCategories
                        .Select(npc => npc.Category.Name)
                        .ToList(),
                    ImagePath = _db.Assets
                        .Where(a => a.ModelType == "news_posts\\news_image" && a.ModelId == n.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} news out of {totalItems} total");

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            return new GetAvailableNewsResponse
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
                "NewsTitle" => isDescending
                    ? query.OrderByDescending(n => n.Title)
                    : query.OrderBy(n => n.Title),

                "CategoryName" => isDescending
                    ? query.OrderByDescending(n => n.NewsPostCategories
                        .Select(npc => npc.Category.Name).FirstOrDefault())
                    : query.OrderBy(n => n.NewsPostCategories
                        .Select(npc => npc.Category.Name).FirstOrDefault()),

                // Default: latest created_at
                _ => query.OrderByDescending(n => n.CreatedAt)
            };
        }
    }
}
