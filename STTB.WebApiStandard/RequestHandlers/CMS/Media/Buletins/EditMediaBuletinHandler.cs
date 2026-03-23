using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Buletins;
using STTB.WebApiStandard.Entities;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Buletins
{
    public class EditMediaBuletinHandler : IRequestHandler<EditMediaBuletinRequest, EditMediaBuletinResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditMediaBuletinHandler> _logger;

        public EditMediaBuletinHandler(SttbDbContext db, ILogger<EditMediaBuletinHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditMediaBuletinResponse> Handle(EditMediaBuletinRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "buletin", ct);

            if (media == null)
                throw new InvalidOperationException($"Buletin {request.Id} not found.");

            media.Title = request.BuletinTitle;
            media.Slug = GenerateSlug(request.BuletinTitle);
            media.Description = request.Description;
            media.PublishedAt = request.PublicationDate;
            media.IsPublished = request.IsPublished;
            media.UpdatedAt = DateTime.UtcNow;

            // Update Categories
            _db.MediaItemTopics.RemoveRange(media.MediaItemTopics);
            if (request.Category != null)
            {
                foreach (var catName in request.Category)
                {
                    var cat = await _db.MediaTopicCategories.FirstOrDefaultAsync(c => c.Name == catName, ct);
                    if (cat == null)
                    {
                        cat = new MediaTopicCategory { Name = catName, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                        await _db.MediaTopicCategories.AddAsync(cat, ct);
                    }
                    media.MediaItemTopics.Add(new MediaItemTopic { TopicCategory = cat, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }
            }

            // Update Authors
            _db.MediaItemWriters.RemoveRange(media.MediaItemWriters);
            if (request.Authors != null)
            {
                foreach (var authorDto in request.Authors)
                {
                    var author = await _db.MediaWriters.FirstOrDefaultAsync(w => w.AuthorName == authorDto.AuthorName && w.AuthorPosition == authorDto.AuthorPosition, ct);
                    if (author == null)
                    {
                        author = new MediaWriter { AuthorName = authorDto.AuthorName, AuthorPosition = authorDto.AuthorPosition, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                        await _db.MediaWriters.AddAsync(author, ct);
                    }
                    media.MediaItemWriters.Add(new MediaItemWriter { MediaWriter = author, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }
            }

            var buletinAsset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"buletins\buletin_file", ct);
            string finalBuletinPath = buletinAsset?.FilePath ?? string.Empty;

            if (request.BuletinFile != null && request.BuletinFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "files");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.BuletinFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.BuletinFile.CopyToAsync(fileStream, ct);
                }
                finalBuletinPath = $"/Uploads/files/{uniqueFileName}";

                if (buletinAsset != null)
                {
                    buletinAsset.FilePath = finalBuletinPath;
                    buletinAsset.FileName = uniqueFileName;
                    buletinAsset.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"buletins\buletin_file",
                        ModelId = media.Id,
                        FileName = uniqueFileName,
                        FilePath = finalBuletinPath,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }, ct);
                }
            }

            var thumbnailAsset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"buletins\buletin_thumbnail", ct);
            string finalThumbnailPath = thumbnailAsset?.FilePath ?? string.Empty;

            if (request.Thumbnail != null && request.Thumbnail.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "images");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.Thumbnail.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Thumbnail.CopyToAsync(fileStream, ct);
                }
                finalThumbnailPath = $"/Uploads/images/{uniqueFileName}";

                if (thumbnailAsset != null)
                {
                    thumbnailAsset.FilePath = finalThumbnailPath;
                    thumbnailAsset.FileName = uniqueFileName;
                    thumbnailAsset.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"buletins\buletin_thumbnail",
                        ModelId = media.Id,
                        FileName = uniqueFileName,
                        FilePath = finalThumbnailPath,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }, ct);
                }
            }

            await _db.SaveChangesAsync(ct);

            return new EditMediaBuletinResponse
            {
                Id = media.Id,
                BuletinTitle = media.Title,
                Description = media.Description ?? string.Empty,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = request.Category ?? Array.Empty<string>(),
                Authors = request.Authors ?? Array.Empty<Contracts.DTOs.CMS.Media.AuthorDTO>(),
                BuletinPath = finalBuletinPath,
                ThumbnailPath = finalThumbnailPath
            };
        }

        private string GenerateSlug(string phrase)
        {
            string str = phrase.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-");
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim('-');
            return str;
        }
    }
}
