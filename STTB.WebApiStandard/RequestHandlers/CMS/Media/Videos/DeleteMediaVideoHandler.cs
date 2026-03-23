using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Videos;
using STTB.WebApiStandard.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Videos
{
    public class DeleteMediaVideoHandler : IRequestHandler<DeleteMediaVideoRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaVideoHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task Handle(DeleteMediaVideoRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemsVideo)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "video", ct);

            if (media == null)
                throw new InvalidOperationException($"Video {request.Id} not found.");

            if (media.MediaItemsVideo != null)
            {
                _db.Remove(media.MediaItemsVideo);
            }

            var asset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"videos\video_thumbnail", ct);
            if (asset != null) _db.Assets.Remove(asset);

            _db.MediaItems.Remove(media);
            await _db.SaveChangesAsync(ct);
        }
    }
}
