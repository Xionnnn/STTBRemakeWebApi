using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Monograf;
using STTB.WebApiStandard.Entities;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Monograf
{
    public class EditMediaMonografHandler : IRequestHandler<EditMediaMonografRequest, EditMediaMonografResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditMediaMonografHandler> _logger;

        public EditMediaMonografHandler(SttbDbContext db, ILogger<EditMediaMonografHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditMediaMonografResponse> Handle(EditMediaMonografRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemsMonograf)
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "monograf", ct);

            if (media == null)
                throw new InvalidOperationException($"Monograf {request.Id} not found.");

            media.Title = request.MonografTitle;
            media.Slug = GenerateSlug(request.MonografTitle);
            media.Description = request.Synopsis;
            media.PublishedAt = DateTime.SpecifyKind(request.PublicationDate, DateTimeKind.Utc);
            media.IsPublished = request.IsPublished;
            media.UpdatedAt = DateTime.UtcNow;

            if (media.MediaItemsMonograf != null)
            {
                media.MediaItemsMonograf.Price = request.Price;
                media.MediaItemsMonograf.Isbn = request.Isbn;
                media.MediaItemsMonograf.Contact = request.Contact;
                media.MediaItemsMonograf.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                media.MediaItemsMonograf = new MediaItemsMonograf
                {
                    Price = request.Price,
                    Isbn = request.Isbn,
                    Contact = request.Contact,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }

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

            var asset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"media_items\monograf_thumbnail", ct);
            string finalThumbnailPath = asset?.FilePath ?? string.Empty;

            if (request.Thumbnail != null && request.Thumbnail.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "images", "media_items");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.Thumbnail.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Thumbnail.CopyToAsync(fileStream, ct);
                }
                finalThumbnailPath = $"/Uploads/images/media_items/{uniqueFileName}";

                if (asset != null)
                {
                    asset.FilePath = finalThumbnailPath;
                    asset.FileName = uniqueFileName;
                    asset.MimeType = request.Thumbnail.ContentType;
                    asset.SizeBytes = request.Thumbnail.Length;
                    asset.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"media_items\monograf_thumbnail",
                        ModelId = media.Id,
                        FileName = uniqueFileName,
                        FilePath = finalThumbnailPath,
                        MimeType = request.Thumbnail.ContentType,
                        SizeBytes = request.Thumbnail.Length,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }, ct);
                }
            }

            await _db.SaveChangesAsync(ct);

            return new EditMediaMonografResponse
            {
                Id = media.Id,
                MonografTitle = media.Title,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = request.Category ?? new List<string>(),
                Authors = request.Authors ?? new List<Contracts.DTOs.CMS.Media.AuthorDTO>(),
                Synopsis = media.Description ?? string.Empty,
                Price = request.Price,
                Isbn = request.Isbn ?? string.Empty,
                Contact = request.Contact ?? string.Empty,
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
