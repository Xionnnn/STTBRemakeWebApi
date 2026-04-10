using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Journals;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Journals
{
    public class AddMediaJournalHandler : IRequestHandler<AddMediaJournalRequest, AddMediaJournalResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddMediaJournalHandler> _logger;

        public AddMediaJournalHandler(SttbDbContext db, ILogger<AddMediaJournalHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddMediaJournalResponse> Handle(AddMediaJournalRequest request, CancellationToken ct)
        {
            var slug = GenerateSlug(request.JournalTitle);

            var media = new MediaItem
            {
                Title = request.JournalTitle,
                Slug = slug,
                MediaFormat = "journal",
                PublishedAt = DateTime.SpecifyKind(request.PublicationDate, DateTimeKind.Utc),
                IsPublished = request.IsPublished,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                MediaItemsJournal = new MediaItemsJournal
                {
                    Abstract = request.Abstract,
                    Issn = request.Issn,
                    EIssn = request.EIssn,
                    Doi = request.Doi,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
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

            string finalJournalPath = string.Empty;
            if (request.JournalFile != null && request.JournalFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "documents", "media_items");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.JournalFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.JournalFile.CopyToAsync(fileStream, ct);
                }
                finalJournalPath = $"/Uploads/documents/media_items/{uniqueFileName}";

                await _db.Assets.AddAsync(new Asset
                {
                    ModelType = @"media_items\journal_content",
                    ModelId = media.Id,
                    FileName = uniqueFileName,
                    FilePath = finalJournalPath,
                    MimeType = request.JournalFile.ContentType,
                    SizeBytes = request.JournalFile.Length,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }, ct);
            }

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
                    ModelType = @"media_items\journal_thumbnail",
                    ModelId = media.Id,
                    FileName = uniqueFileName,
                    FilePath = finalThumbnailPath,
                    MimeType = request.Thumbnail.ContentType,
                    SizeBytes = request.Thumbnail.Length,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }, ct);
            }

            if (request.JournalFile != null || request.Thumbnail != null)
            {
                await _db.SaveChangesAsync(ct);
            }

            _logger.LogInformation("Journal {Id} created successfully.", media.Id);

            return new AddMediaJournalResponse
            {
                Id = media.Id,
                JournalTitle = media.Title,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
                Abstract = request.Abstract,
                Category = request.Category ?? new List<string>(),
                Authors = request.Authors ?? new List<Contracts.DTOs.CMS.Media.AuthorDTO>(),
                Issn = request.Issn ?? string.Empty,
                EIssn = request.EIssn ?? string.Empty,
                Doi = request.Doi ?? string.Empty,
                JournalPath = finalJournalPath,
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
