using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Journals;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Journals
{
    public class GetMediaJournalHandler : IRequestHandler<GetMediaJournalRequest, GetMediaJournalResponse>
    {
        private readonly SttbDbContext _db;

        public GetMediaJournalHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetMediaJournalResponse> Handle(GetMediaJournalRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemsJournal)
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "journal", ct);

            if (media == null)
                throw new InvalidOperationException($"Journal {request.Id} not found.");

            var journalAsset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"media_items\journal_content", ct);
            var thumbnailAsset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"media_items\journal_thumbnail", ct);

            return new GetMediaJournalResponse
            {
                Id = media.Id,
                JournalTitle = media.Title,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = media.MediaItemTopics.Select(mt => mt.TopicCategory.Name).ToList(),
                Authors = media.MediaItemWriters.Select(mw => new Contracts.DTOs.CMS.Media.AuthorDTO
                {
                    AuthorName = mw.MediaWriter.AuthorName,
                    AuthorPosition = mw.MediaWriter.AuthorPosition ?? string.Empty
                }).ToList(),
                Issn = media.MediaItemsJournal?.Issn ?? string.Empty,
                EIssn = media.MediaItemsJournal?.EIssn ?? string.Empty,
                Doi = media.MediaItemsJournal?.Doi ?? string.Empty,
                JournalPath = journalAsset?.FilePath ?? string.Empty,
                ThumbnailPath = thumbnailAsset?.FilePath ?? string.Empty
            };
        }
    }
}
