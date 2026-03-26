using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News
{
    public class AddNewsHandler : IRequestHandler<AddNewsRequest, AddNewsResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddNewsHandler> _logger;

        public AddNewsHandler(SttbDbContext db, ILogger<AddNewsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddNewsResponse> Handle(AddNewsRequest request, CancellationToken ct)
        {
            var news = new NewsPost
            {
                Slug = request.Slug,
                Title = request.Title,
                Content = request.Content,
                PublishedAt = DateTime.SpecifyKind(request.PublicationDate, DateTimeKind.Utc),
                IsPublished = request.IsPublished,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.NewsPosts.AddAsync(news, ct);

            // Handle Categories
            if (request.Category != null && request.Category.Any())
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
                        await _db.SaveChangesAsync(ct);
                    }

                    news.NewsPostCategories.Add(new NewsPostCategory
                    {
                        NewsPost = news,
                        CategoryId = cat.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }

            await _db.SaveChangesAsync(ct);

            // Handle Image Upload
            string finalImagePath = string.Empty;
            if (request.NewsImage != null && request.NewsImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "images", "news");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.NewsImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.NewsImage.CopyToAsync(fileStream, ct);
                }

                finalImagePath = $"/Uploads/images/news/{uniqueFileName}";

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

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("News {Id} created successfully.", news.Id);

            return new AddNewsResponse
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
