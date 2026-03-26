using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News
{
    public class EditNewsHandler : IRequestHandler<EditNewsRequest, EditNewsResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditNewsHandler> _logger;

        public EditNewsHandler(SttbDbContext db, ILogger<EditNewsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditNewsResponse> Handle(EditNewsRequest request, CancellationToken ct)
        {
            var news = await _db.NewsPosts
                .Include(n => n.NewsPostCategories)
                .FirstOrDefaultAsync(n => n.Id == request.Id, ct);

            if (news == null)
            {
                throw new KeyNotFoundException($"News Post with ID {request.Id} was not found.");
            }

            // Update basic fields
            news.Slug = request.Slug;
            news.Title = request.Title;
            news.Content = request.Content;
            news.PublishedAt = DateTime.SpecifyKind(request.PublicationDate, DateTimeKind.Utc);
            news.IsPublished = request.IsPublished;
            news.UpdatedAt = DateTime.UtcNow;

            // Handle Categories
            _db.NewsPostCategories.RemoveRange(news.NewsPostCategories);
            
            if (request.Category != null)
            {
                foreach (var categoryName in request.Category)
                {
                    if (string.IsNullOrWhiteSpace(categoryName)) continue;

                    var cat = await _db.NewsCategories
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower(), ct);

                    if (cat == null)
                    {
                        cat = new NewsCategory
                        {
                            Name = categoryName,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _db.NewsCategories.AddAsync(cat, ct);
                        await _db.SaveChangesAsync(ct); // Save to generate ID
                    }

                    news.NewsPostCategories.Add(new NewsPostCategory
                    {
                        NewsPostId = news.Id,
                        CategoryId = cat.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            // Handle Image Upload
            string finalImagePath = string.Empty;
            if (request.NewsImage != null && request.NewsImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "images", "news_posts");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.NewsImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.NewsImage.CopyToAsync(fileStream, ct);
                }

                finalImagePath = $"/Uploads/images/news_posts/{uniqueFileName}";

                var existingAsset = await _db.Assets
                    .FirstOrDefaultAsync(a => a.ModelType == @"news_posts\news_image" && a.ModelId == news.Id, ct);

                if (existingAsset != null)
                {
                    var oldPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingAsset.FilePath.TrimStart('/'));
                    if (File.Exists(oldPhysicalPath)) File.Delete(oldPhysicalPath);

                    existingAsset.FilePath = finalImagePath;
                    existingAsset.FileName = uniqueFileName;
                    existingAsset.MimeType = request.NewsImage.ContentType;
                    existingAsset.SizeBytes = request.NewsImage.Length;
                    existingAsset.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    await _db.Assets.AddAsync(new Asset
                    {
                        ModelType = @"news_posts\news_image",
                        ModelId = news.Id,
                        FileName = uniqueFileName,
                        FilePath = finalImagePath,
                        MimeType = request.NewsImage.ContentType,
                        SizeBytes = request.NewsImage.Length,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }, ct);
                }
            }
            else
            {
                finalImagePath = await _db.Assets
                    .Where(a => a.ModelType == @"news_posts\news_image" && a.ModelId == news.Id)
                    .Select(a => a.FilePath)
                    .FirstOrDefaultAsync(ct) ?? string.Empty;
            }

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("News Post {Id} updated successfully.", news.Id);

            return new EditNewsResponse
            {
                Id = news.Id,
                Slug = news.Slug,
                Title = news.Title,
                Content = news.Content,
                PublicationDate = news.PublishedAt,
                Category = request.Category ?? new List<string>(),
                ImagePath = finalImagePath,
                IsPublished = news.IsPublished
            };
        }
    }
}
