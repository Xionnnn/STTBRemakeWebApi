using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events.Categories
{
    public class EditEventCategoryHandler : IRequestHandler<EditEventCategoryRequest, EditEventCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditEventCategoryHandler> _logger;

        public EditEventCategoryHandler(SttbDbContext db, ILogger<EditEventCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditEventCategoryResponse> Handle(EditEventCategoryRequest request, CancellationToken ct)
        {
            var eventCategory = await _db.EventCategories
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (eventCategory == null)
            {
                throw new Exception("Data doesnt exist");
            }

            eventCategory.Name = request.CategoryName;
            eventCategory.Slug = request.Slug;
            eventCategory.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully updated EventCategory {Id}.", request.Id);

            return new EditEventCategoryResponse
            {
                Id = request.Id,
                CategoryName = request.CategoryName,
                Slug = request.Slug
            };
        }
    }
}
