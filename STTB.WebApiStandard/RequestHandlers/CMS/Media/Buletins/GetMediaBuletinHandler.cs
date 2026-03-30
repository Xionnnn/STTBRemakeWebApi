using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Buletins;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Buletins
{
    public class GetMediaBuletinHandler : IRequestHandler<GetMediaBuletinRequest, GetMediaBuletinResponse>
    {
        private readonly SttbDbContext _db;

        public GetMediaBuletinHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetMediaBuletinResponse> Handle(GetMediaBuletinRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "buletin", ct);

            if (media == null)
                throw new InvalidOperationException($"Buletin {request.Id} not found.");

            var buletinAsset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"media_items\buletin_content", ct);
            var thumbnailAsset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"media_items\buletin_thumbnail", ct);

            return new GetMediaBuletinResponse
            {
                Id = media.Id,
                BuletinTitle = media.Title,
                Description = media.Description ?? string.Empty,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = media.MediaItemTopics.Select(mt => mt.TopicCategory.Name).ToList(),
                Authors = media.MediaItemWriters.Select(mw => new Contracts.DTOs.CMS.Media.AuthorDTO
                {
                    AuthorName = mw.MediaWriter.AuthorName,
                    AuthorPosition = mw.MediaWriter.AuthorPosition ?? string.Empty
                }).ToList(),
                BuletinPath = buletinAsset?.FilePath ?? string.Empty,
                ThumbnailPath = thumbnailAsset?.FilePath ?? string.Empty
            };
        }
    }
}
