using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Videos;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Videos;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Videos
{
    public class GetMediaVideoHandler : IRequestHandler<GetMediaVideoRequest, GetMediaVideoResponse>
    {
        private readonly SttbDbContext _db;

        public GetMediaVideoHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetMediaVideoResponse> Handle(GetMediaVideoRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemsVideo)
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "video", ct);

            if (media == null)
                throw new InvalidOperationException($"Video {request.Id} not found.");

            var asset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"media_items\video_thumbnail", ct);

            return new GetMediaVideoResponse
            {
                Id = media.Id,
                VideoTitle = media.Title,
                VideoDescription = media.Description ?? string.Empty,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = media.MediaItemTopics.Select(mt => mt.TopicCategory.Name).ToList(),
                Authors = media.MediaItemWriters.Select(mw => new Contracts.DTOs.CMS.Media.AuthorDTO
                {
                    AuthorName = mw.MediaWriter.AuthorName,
                    AuthorPosition = mw.MediaWriter.AuthorPosition ?? string.Empty
                }).ToList(),
                VideoUrl = media.MediaItemsVideo?.VideoUrl ?? string.Empty,
                ThumbnailPath = asset?.FilePath ?? string.Empty
            };
        }
    }
}
