using MediatR;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Categories
{
    public class AddMediaCategoryHandler : IRequestHandler<AddMediaCategoryRequest, AddMediaCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddMediaCategoryHandler> _logger;

        public AddMediaCategoryHandler(SttbDbContext db, ILogger<AddMediaCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddMediaCategoryResponse> Handle(AddMediaCategoryRequest request, CancellationToken ct)
        {
            var category = new MediaTopicCategory
            {
                Name = request.CategoryName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.MediaTopicCategories.AddAsync(category, ct);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully created MediaCategory {Id}.", category.Id);

            return new AddMediaCategoryResponse
            {
                Id = category.Id,
                CategoryName = category.Name
            };
        }
    }
}
