using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Web.Media
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
                .Include(m => m.MediaItemWriters)
                    .ThenInclude(mw => mw.MediaWriter)
                .Include(m => m.MediaItemsJournal)
                .Where(m => m.IsPublished && m.MediaFormat.ToLower() == "journal" && m.Slug == request.JournalSlug)
                .Select(m => new GetJournalDetailResponse
                {
                    Id = m.Id,
                    JournalTitle = m.Title,
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
                    Abstract = m.MediaItemsJournal != null ? m.MediaItemsJournal.Abstract : string.Empty,
                    JournalPath = _db.Assets
                        .Where(a => a.ModelType == "media_items\\journal_content" && a.ModelId == m.Id)
                        .Select(a => a.FilePath)
                        .FirstOrDefault() ?? string.Empty,
                    Issn = m.MediaItemsJournal != null ? m.MediaItemsJournal.Issn ?? string.Empty : string.Empty,
                    EIssn = m.MediaItemsJournal != null ? m.MediaItemsJournal.EIssn ?? string.Empty : string.Empty,
                    Doi = m.MediaItemsJournal != null ? m.MediaItemsJournal.Doi ?? string.Empty : string.Empty
                })
                .FirstOrDefaultAsync(ct);

            return journal;
        }
    }
}
