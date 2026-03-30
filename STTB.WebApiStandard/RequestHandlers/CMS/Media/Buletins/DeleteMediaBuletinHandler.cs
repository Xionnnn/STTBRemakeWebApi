using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Buletins
{
    public class DeleteMediaBuletinHandler : IRequestHandler<DeleteMediaBuletinRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaBuletinHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task Handle(DeleteMediaBuletinRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems.FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "buletin", ct);
            if (media == null)
                throw new InvalidOperationException($"Buletin {request.Id} not found.");

            var assets = await _db.Assets.Where(a => a.ModelId == media.Id && (a.ModelType == @"media_items\buletin_content" || a.ModelType == @"media_items\buletin_thumbnail")).ToListAsync(ct);
            if (assets.Any()) _db.Assets.RemoveRange(assets);

            _db.MediaItems.Remove(media);
            await _db.SaveChangesAsync(ct);
        }
    }
}
