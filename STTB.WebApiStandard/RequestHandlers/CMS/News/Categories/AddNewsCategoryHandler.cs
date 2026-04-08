using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News.Categories
{
    public class AddNewsCategoryHandler : IRequestHandler<AddNewsCategoryRequest, AddNewsCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddNewsCategoryHandler> _logger;

        public AddNewsCategoryHandler(SttbDbContext db, ILogger<AddNewsCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddNewsCategoryResponse> Handle(AddNewsCategoryRequest request, CancellationToken ct)
        {
            var newsCategory = new NewsCategory
            {
                Name = request.CategoryName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.NewsCategories.AddAsync(newsCategory, ct);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully created NewsCategory {Id}.", newsCategory.Id);

            return new AddNewsCategoryResponse
            {
                Id = newsCategory.Id,
                CategoryName = newsCategory.Name
            };
        }
    }
}
