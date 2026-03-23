using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles;
using STTB.WebApiStandard.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Articles
{
    public class DeleteMediaArticleHandler : IRequestHandler<DeleteMediaArticleRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaArticleHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task Handle(DeleteMediaArticleRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems.FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "Artikel", ct);
            if (media == null)
                throw new InvalidOperationException($"Article {request.Id} not found.");

            // Assets table cascades/cleans up through FK or should be deleted manually.
            var asset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"articles\article_thumbnail", ct);
            if (asset != null) _db.Assets.Remove(asset);

            _db.MediaItems.Remove(media);
            await _db.SaveChangesAsync(ct);
        }
    }
}
