using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Media
{
    public class GetArticleDetailHandler : IRequestHandler<GetArticleDetailRequest, GetArticleDetailResponse>
    {
        private readonly SttbDbContext _db;

        public GetArticleDetailHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetArticleDetailResponse> Handle(GetArticleDetailRequest request, CancellationToken ct)
        {
            var article = await _db.MediaItems
                .Include(m => m.MediaItemTopics)
                    .ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters)
                    .ThenInclude(mw => mw.MediaWriter)
                .Where(m => m.IsPublished && m.MediaFormat.ToLower() == "article" && m.Slug == request.ArticleSlug)
                .Select(m => new GetArticleDetailResponse
                {
                    Id = m.Id,
                    ArticleTitle = m.Title,
                    ArticleDescription = m.Description ?? string.Empty,
                    PublicationDate = m.PublishedAt,
                    Category = m.MediaItemTopics
                        .Select(mt => mt.TopicCategory.Name)
                        .ToList(),
                    Authors = m.MediaItemWriters
                        .Select(mw => new AuthorDTO
                        {
                            AuthorName = mw.MediaWriter.AuthorName,
                            AuthorPosition = mw.MediaWriter.AuthorPosition ?? string.Empty
                        })
                        .ToList(),
                    ArticleContent = m.Content ?? string.Empty,
                    ThumbnailPath = _db.Assets
                        .Where(a => a.ModelType == "media_items\\article_thumbnail" && a.ModelId == m.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .FirstOrDefaultAsync(ct);

            return article;
        }
    }
}
