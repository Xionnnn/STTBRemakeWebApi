using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.News;
using STTB.WebApiStandard.Contracts.ResponseModels.News;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Web.News
{
    public class GetNewsHandler : IRequestHandler<GetNewsRequest, GetNewsResponse?>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetNewsHandler> _logger;
        public GetNewsHandler(SttbDbContext db, ILogger<GetNewsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetNewsResponse?> Handle(GetNewsRequest request, CancellationToken ct)
        {
            var news = await _db.NewsPosts
                .Include(n => n.NewsPostCategories)
                    .ThenInclude(npc => npc.Category)
                .Where(n => n.Slug == request.NewsSlug && n.IsPublished)
                .AsNoTracking()
                .Select(n => new GetNewsResponse
                {
                    Id = n.Id,
                    Slug = n.Slug,
                    Title = n.Title,
                    Content = n.Content,
                    PublicationDate = n.PublishedAt,
                    Category = n.NewsPostCategories
                        .Select(npc => npc.Category.Name)
                        .ToList(),
                    ImagePath = _db.Assets
                        .Where(a => a.ModelType == "news_posts\\news_image" && a.ModelId == n.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .FirstOrDefaultAsync(ct);

            if (news == null)
            {
                _logger.LogInformation($"News with slug '{request.NewsSlug}' not found");
            }

            return news;
        }
    }
}
