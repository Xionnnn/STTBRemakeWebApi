using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using STTB.WebApiStandard.Contracts.DTOs.Web.Media;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Web.Media
{
    public class GetBuletinDetailHandler : IRequestHandler<GetBuletinDetailRequest, GetBuletinDetailResponse>
    {
        private readonly SttbDbContext _db;

        public GetBuletinDetailHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetBuletinDetailResponse> Handle(GetBuletinDetailRequest request, CancellationToken ct)
        {
            var buletin = await _db.MediaItems
                .Include(m => m.MediaItemTopics)
                    .ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters)
                    .ThenInclude(mw => mw.MediaWriter)
                .Where(m => m.IsPublished && m.MediaFormat.ToLower() == "buletin" && m.Slug == request.BuletinSlug)
                .Select(m => new GetBuletinDetailResponse
                {
                    Id = m.Id,
                    BuletinTitle = m.Title,
                    Description = m.Description ?? string.Empty,
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
                    BuletinPath = _db.Assets
                        .Where(a => a.ModelType == "media_items\\buletin_content" && a.ModelId == m.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .FirstOrDefaultAsync(ct);

            return buletin;
        }
    }
}
