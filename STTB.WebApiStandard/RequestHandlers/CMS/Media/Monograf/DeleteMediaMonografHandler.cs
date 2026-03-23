using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf;
using STTB.WebApiStandard.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Monograf
{
    public class DeleteMediaMonografHandler : IRequestHandler<DeleteMediaMonografRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaMonografHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task Handle(DeleteMediaMonografRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemsMonograf)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "monograf", ct);

            if (media == null)
                throw new InvalidOperationException($"Monograf {request.Id} not found.");

            if (media.MediaItemsMonograf != null)
            {
                _db.Remove(media.MediaItemsMonograf);
            }

            var asset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"monografs\monograf_thumbnail", ct);
            if (asset != null) _db.Assets.Remove(asset);

            _db.MediaItems.Remove(media);
            await _db.SaveChangesAsync(ct);
        }
    }
}
