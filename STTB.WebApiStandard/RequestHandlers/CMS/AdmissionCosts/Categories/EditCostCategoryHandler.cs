using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts.Categories
{
    public class EditCostCategoryHandler : IRequestHandler<EditCostCategoryRequest, EditCostCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditCostCategoryHandler> _logger;

        public EditCostCategoryHandler(SttbDbContext db, ILogger<EditCostCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditCostCategoryResponse> Handle(EditCostCategoryRequest request, CancellationToken ct)
        {
            var category = await _db.AcademicProgramCostCategories
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            if (category == null)
            {
                throw new Exception("Data doesnt exist");
            }

            category.CategoryName = request.CategoryName;
            category.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully updated AcademicProgramCostCategory {Id}.", request.Id);

            return new EditCostCategoryResponse
            {
                Id = request.Id,
                CategoryName = request.CategoryName
            };
        }
    }
}
