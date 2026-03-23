using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Monograf;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Monograf
{
    public class GetMediaMonografHandler : IRequestHandler<GetMediaMonografRequest, GetMediaMonografResponse>
    {
        private readonly SttbDbContext _db;

        public GetMediaMonografHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetMediaMonografResponse> Handle(GetMediaMonografRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemsMonograf)
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "Monograf", ct);

            if (media == null)
                throw new InvalidOperationException($"Monograf {request.Id} not found.");

            var asset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"monografs\monograf_thumbnail", ct);

            return new GetMediaMonografResponse
            {
                Id = media.Id,
                MonografTitle = media.Title,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = media.MediaItemTopics.Select(mt => mt.TopicCategory.Name).ToList(),
                Authors = media.MediaItemWriters.Select(mw => new Contracts.DTOs.CMS.Media.AuthorDTO
                {
                    AuthorName = mw.MediaWriter.AuthorName,
                    AuthorPosition = mw.MediaWriter.AuthorPosition ?? string.Empty
                }).ToList(),
                Synopsis = media.Description ?? string.Empty,
                Price = media.MediaItemsMonograf?.Price ?? 0,
                Isbn = media.MediaItemsMonograf?.Isbn ?? string.Empty,
                Contact = media.MediaItemsMonograf?.Contact ?? string.Empty,
                ThumbnailPath = asset?.FilePath ?? string.Empty
            };
        }
    }
}
