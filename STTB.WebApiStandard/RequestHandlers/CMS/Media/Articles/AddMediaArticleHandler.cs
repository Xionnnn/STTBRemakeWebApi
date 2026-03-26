using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Articles;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Articles
{
    public class AddMediaArticleHandler : IRequestHandler<AddMediaArticleRequest, AddMediaArticleResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddMediaArticleHandler> _logger;

        public AddMediaArticleHandler(SttbDbContext db, ILogger<AddMediaArticleHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddMediaArticleResponse> Handle(AddMediaArticleRequest request, CancellationToken ct)
        {
            var slug = GenerateSlug(request.ArticleTitle);

            var media = new MediaItem
            {
                Title = request.ArticleTitle,
                Slug = slug,
                Description = request.ArticleDescription,
                Content = request.ArticleContent,
                MediaFormat = "article",
                PublishedAt = DateTime.SpecifyKind(request.PublicationDate, DateTimeKind.Utc),
                IsPublished = request.IsPublished,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.MediaItems.AddAsync(media, ct);

            // Handle Categories
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

            // Handle Authors
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

            await _db.SaveChangesAsync(ct);

            string finalThumbnailPath = string.Empty;
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

                await _db.Assets.AddAsync(new Asset
                {
                    ModelType = @"articles\article_thumbnail",
                    ModelId = media.Id,
                    FileName = uniqueFileName,
                    FilePath = finalThumbnailPath,
                    MimeType = request.Thumbnail.ContentType,
                    SizeBytes = request.Thumbnail.Length,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }, ct);
                await _db.SaveChangesAsync(ct);
            }

            _logger.LogInformation("Article {Id} created successfully.", media.Id);

            return new AddMediaArticleResponse
            {
                Id = media.Id,
                ArticleTitle = media.Title,
                ArticleDescription = media.Description ?? string.Empty,
                ArticleContent = media.Content ?? string.Empty,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Category = request.Category ?? new List<string>(),
                Authors = request.Authors ?? new List<Contracts.DTOs.CMS.Media.AuthorDTO>(),
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
