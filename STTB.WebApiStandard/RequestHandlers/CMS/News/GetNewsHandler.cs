using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News
{
    public class GetNewsHandler : IRequestHandler<GetNewsRequest, GetNewsResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetNewsHandler> _logger;

        public GetNewsHandler(SttbDbContext db, ILogger<GetNewsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetNewsResponse> Handle(GetNewsRequest request, CancellationToken ct)
        {
            var news = await _db.NewsPosts
                .Include(n => n.NewsPostCategories)
                    .ThenInclude(npc => npc.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == request.Id, ct);

            if (news == null)
            {
                throw new KeyNotFoundException($"News Post with ID {request.Id} was not found.");
            }

            var asset = await _db.Assets
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ModelType == @"news_posts\news_image" && a.ModelId == news.Id, ct);

            return new GetNewsResponse
            {
                Id = news.Id,
                Slug = news.Slug,
                Title = news.Title,
                Content = news.Content,
                PublicationDate = news.PublishedAt,
                Category = news.NewsPostCategories.Select(npc => npc.Category.Name).ToList(),
                IsPublished = news.IsPublished,
                ImagePath = asset?.FilePath ?? string.Empty
            };
        }
    }
}
