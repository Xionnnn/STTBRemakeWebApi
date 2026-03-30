using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Journals
{
    public class DeleteMediaJournalHandler : IRequestHandler<DeleteMediaJournalRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaJournalHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task Handle(DeleteMediaJournalRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemsJournal)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "journal", ct);

            if (media == null)
                throw new InvalidOperationException($"Journal {request.Id} not found.");

            if (media.MediaItemsJournal != null)
            {
                _db.Remove(media.MediaItemsJournal);
            }

            var assets = await _db.Assets.Where(a => a.ModelId == media.Id && (a.ModelType == @"media_items\journal_content" || a.ModelType == @"media_items\journal_thumbnail")).ToListAsync(ct);
            if (assets.Any()) _db.Assets.RemoveRange(assets);

            _db.MediaItems.Remove(media);
            await _db.SaveChangesAsync(ct);
        }
    }
}
