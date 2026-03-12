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
    public class GetJournalDetailHandler : IRequestHandler<GetJournalDetailRequest, GetJournalDetailResponse>
    {
        private readonly SttbDbContext _db;

        public GetJournalDetailHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetJournalDetailResponse> Handle(GetJournalDetailRequest request, CancellationToken ct)
        {
            var journal = await _db.MediaItems
                .Include(m => m.MediaItemTopics)
                    .ThenInclude(mt => mt.TopicCategory)
                .Where(m => m.IsPublished && m.MediaFormat.ToLower() == "journal" && m.Slug == request.JournalSlug)
                .Select(m => new GetJournalDetailResponse
                {
                    Id = m.Id,
                    JournalTitle = m.Title,
                    AuthorName = m.AuthorName ?? string.Empty,
                    JournalDescription = m.Description ?? string.Empty,
                    Theme = m.Theme ?? string.Empty,
                    PublicationDate = m.PublishedAt,
                    Category = m.MediaItemTopics
                        .Select(mt => mt.TopicCategory.Name)
                        .ToList(),
                    JournalPath = _db.Assets
                        .Where(a => a.ModelType == "media_items\\journal_content" && a.ModelId == m.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty
                })
                .FirstOrDefaultAsync(ct);

            return journal;
        }
    }
}
