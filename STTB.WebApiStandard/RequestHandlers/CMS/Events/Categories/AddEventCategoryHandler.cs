using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events.Categories
{
    public class AddEventCategoryHandler : IRequestHandler<AddEventCategoryRequest, AddEventCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddEventCategoryHandler> _logger;

        public AddEventCategoryHandler(SttbDbContext db, ILogger<AddEventCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddEventCategoryResponse> Handle(AddEventCategoryRequest request, CancellationToken ct)
        {
            var eventCategory = new EventCategory
            {
                Name = request.CategoryName,
                Slug = request.Slug,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.EventCategories.AddAsync(eventCategory, ct);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully created EventCategory {Id}.", eventCategory.Id);

            return new AddEventCategoryResponse
            {
                Id = eventCategory.Id,
                CategoryName = eventCategory.Name,
                Slug = eventCategory.Slug
            };
        }
    }
}
