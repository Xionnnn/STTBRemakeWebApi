using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Articles;
using STTB.WebApiStandard.Entities;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Articles
{
    public class EditMediaArticleHandler : IRequestHandler<EditMediaArticleRequest, EditMediaArticleResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditMediaArticleHandler> _logger;

        public EditMediaArticleHandler(SttbDbContext db, ILogger<EditMediaArticleHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditMediaArticleResponse> Handle(EditMediaArticleRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "article", ct);

            if (media == null)
                throw new InvalidOperationException($"Article {request.Id} not found.");

            media.Title = request.ArticleTitle;
            media.Slug = GenerateSlug(request.ArticleTitle);
            media.Description = request.ArticleContent;
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

            var asset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"articles\article_thumbnail", ct);
            string finalThumbnailPath = asset?.FilePath ?? string.Empty;

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

                if (asset != null)
                {
                    asset.FilePath = finalThumbnailPath;
                    asset.FileName = uniqueFileName;
                    asset.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"articles\article_thumbnail",
                        ModelId = media.Id,
                        FileName = uniqueFileName,
                        FilePath = finalThumbnailPath,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }, ct);
                }
            }

            await _db.SaveChangesAsync(ct);

            return new EditMediaArticleResponse
            {
                Id = media.Id,
                ArticleTitle = media.Title,
                ArticleContent = media.Description ?? string.Empty,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = request.Category ?? Array.Empty<string>(),
                Authors = request.Authors ?? Array.Empty<Contracts.DTOs.CMS.Media.AuthorDTO>(),
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
