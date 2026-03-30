using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Articles;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Articles
{
    public class GetMediaArticleHandler : IRequestHandler<GetMediaArticleRequest, GetMediaArticleResponse>
    {
        private readonly SttbDbContext _db;

        public GetMediaArticleHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetMediaArticleResponse> Handle(GetMediaArticleRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "article", ct);

            if (media == null)
                throw new InvalidOperationException($"Article {request.Id} not found.");

            var asset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"media_items\article_thumbnail", ct);

            return new GetMediaArticleResponse
            {
                Id = media.Id,
                ArticleTitle = media.Title,
                ArticleContent = media.Description ?? string.Empty,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = media.MediaItemTopics.Select(mt => mt.TopicCategory.Name).ToList(),
                Authors = media.MediaItemWriters.Select(mw => new Contracts.DTOs.CMS.Media.AuthorDTO
                {
                    AuthorName = mw.MediaWriter.AuthorName,
                    AuthorPosition = mw.MediaWriter.AuthorPosition ?? string.Empty
                }).ToList(),
                ThumbnailPath = asset?.FilePath ?? string.Empty
            };
        }
    }
}
