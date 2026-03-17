using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Media
{
    public class GetVideoDetailHandler : IRequestHandler<GetVideoDetailRequest, GetVideoDetailResponse>
    {
        private readonly SttbDbContext _db;

        public GetVideoDetailHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetVideoDetailResponse> Handle(GetVideoDetailRequest request, CancellationToken ct)
        {
            var video = await _db.MediaItems
                .Include(m => m.MediaItemTopics)
                    .ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters)
                    .ThenInclude(mw => mw.MediaWriter)
                .Include(m => m.MediaItemsVideo)
                .Where(m => m.IsPublished && m.MediaFormat.ToLower() == "video" && m.Slug == request.VideoSlug)
                .Select(m => new GetVideoDetailResponse
                {
                    Id = m.Id,
                    VideoTitle = m.Title,
                    VideoDescription = m.Description ?? string.Empty,
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
                    VideoUrl = m.MediaItemsVideo != null ? m.MediaItemsVideo.VideoUrl : string.Empty
                })
                .FirstOrDefaultAsync(ct);

            return video;
        }
    }
}
