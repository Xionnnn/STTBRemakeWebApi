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
    public class GetMonografDetailHandler : IRequestHandler<GetMonografDetailRequest, GetMonografDetailResponse>
    {
        private readonly SttbDbContext _db;

        public GetMonografDetailHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetMonografDetailResponse> Handle(GetMonografDetailRequest request, CancellationToken ct)
        {
            var monograf = await _db.MediaItems
                .Include(m => m.MediaItemTopics)
                    .ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters)
                    .ThenInclude(mw => mw.MediaWriter)
                .Include(m => m.MediaItemsMonograf)
                .Where(m => m.IsPublished && m.MediaFormat.ToLower() == "monograf" && m.Slug == request.MonografSlug)
                .Select(m => new GetMonografDetailResponse
                {
                    Id = m.Id,
                    MonografTitle = m.Title,
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
                    Synopsis = m.Description ?? string.Empty,
                    Price = m.MediaItemsMonograf != null ? m.MediaItemsMonograf.Price ?? 0 : 0,
                    Isbn = m.MediaItemsMonograf != null ? m.MediaItemsMonograf.Isbn ?? string.Empty : string.Empty,
                    Contact = m.MediaItemsMonograf != null ? m.MediaItemsMonograf.Contact ?? string.Empty : string.Empty,
                    ThumbnailPath = _db.Assets
                        .Where(a => a.ModelType == "media_items\\monograf_thumbnail" && a.ModelId == m.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .FirstOrDefaultAsync(ct);

            return monograf;
        }
    }
}
