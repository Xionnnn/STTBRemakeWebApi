using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Journals;
using STTB.WebApiStandard.Entities;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Journals
{
    public class EditMediaJournalHandler : IRequestHandler<EditMediaJournalRequest, EditMediaJournalResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditMediaJournalHandler> _logger;

        public EditMediaJournalHandler(SttbDbContext db, ILogger<EditMediaJournalHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditMediaJournalResponse> Handle(EditMediaJournalRequest request, CancellationToken ct)
        {
            var media = await _db.MediaItems
                .Include(m => m.MediaItemsJournal)
                .Include(m => m.MediaItemTopics).ThenInclude(mt => mt.TopicCategory)
                .Include(m => m.MediaItemWriters).ThenInclude(mw => mw.MediaWriter)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "journal", ct);

            if (media == null)
                throw new InvalidOperationException($"Journal {request.Id} not found.");

            media.Title = request.JournalTitle;
            media.Slug = GenerateSlug(request.JournalTitle);
            media.PublishedAt = DateTime.SpecifyKind(request.PublicationDate, DateTimeKind.Utc);
            media.IsPublished = request.IsPublished;
            media.UpdatedAt = DateTime.UtcNow;

            if (media.MediaItemsJournal != null)
            {
                media.MediaItemsJournal.Issn = request.Issn;
                media.MediaItemsJournal.EIssn = request.EIssn;
                media.MediaItemsJournal.Doi = request.Doi;
                media.MediaItemsJournal.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                media.MediaItemsJournal = new MediaItemsJournal
                {
                    Issn = request.Issn,
                    EIssn = request.EIssn,
                    Doi = request.Doi,
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

            var journalAsset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"journals\journal_file", ct);
            string finalJournalPath = journalAsset?.FilePath ?? string.Empty;

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

                if (journalAsset != null)
                {
                    journalAsset.FilePath = finalJournalPath;
                    journalAsset.FileName = uniqueFileName;
                    journalAsset.MimeType = request.JournalFile.ContentType;
                    journalAsset.SizeBytes = request.JournalFile.Length;
                    journalAsset.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"journals\journal_file",
                        ModelId = media.Id,
                        FileName = uniqueFileName,
                        FilePath = finalJournalPath,
                        MimeType = request.JournalFile.ContentType,
                        SizeBytes = request.JournalFile.Length,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }, ct);
                }
            }

            var thumbnailAsset = await _db.Assets.FirstOrDefaultAsync(a => a.ModelId == media.Id && a.ModelType == @"journals\journal_thumbnail", ct);
            string finalThumbnailPath = thumbnailAsset?.FilePath ?? string.Empty;

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

                if (thumbnailAsset != null)
                {
                    thumbnailAsset.FilePath = finalThumbnailPath;
                    thumbnailAsset.FileName = uniqueFileName;
                    thumbnailAsset.MimeType = request.Thumbnail.ContentType;
                    thumbnailAsset.SizeBytes = request.Thumbnail.Length;
                    thumbnailAsset.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"journals\journal_thumbnail",
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

            return new EditMediaJournalResponse
            {
                Id = media.Id,
                JournalTitle = media.Title,
                PublicationDate = media.PublishedAt,
                IsPublished = media.IsPublished,
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
