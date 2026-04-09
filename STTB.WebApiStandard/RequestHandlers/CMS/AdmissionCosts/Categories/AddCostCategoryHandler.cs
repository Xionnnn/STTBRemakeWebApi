using MediatR;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts.Categories
{
    public class AddCostCategoryHandler : IRequestHandler<AddCostCategoryRequest, AddCostCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddCostCategoryHandler> _logger;

        public AddCostCategoryHandler(SttbDbContext db, ILogger<AddCostCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddCostCategoryResponse> Handle(AddCostCategoryRequest request, CancellationToken ct)
        {
            var category = new AcademicProgramCostCategory
            {
                CategoryName = request.CategoryName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.AcademicProgramCostCategories.AddAsync(category, ct);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully created AcademicProgramCostCategory {Id}.", category.Id);

            return new AddCostCategoryResponse
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
        }
    }
}
