using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Categories
{
    public class EditMediaCategoryHandler : IRequestHandler<EditMediaCategoryRequest, EditMediaCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditMediaCategoryHandler> _logger;

        public EditMediaCategoryHandler(SttbDbContext db, ILogger<EditMediaCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditMediaCategoryResponse> Handle(EditMediaCategoryRequest request, CancellationToken ct)
        {
            var category = await _db.MediaTopicCategories
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            if (category == null)
            {
                throw new Exception("Data doesnt exist");
            }

            category.Name = request.CategoryName;
            category.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully updated MediaCategory {Id}.", request.Id);

            return new EditMediaCategoryResponse
            {
                Id = request.Id,
                CategoryName = request.CategoryName
            };
        }
    }
}
