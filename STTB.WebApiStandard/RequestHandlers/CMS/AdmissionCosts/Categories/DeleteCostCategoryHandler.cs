using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts.Categories
{
    public class DeleteCostCategoryHandler : IRequestHandler<DeleteCostCategoryRequest, DeleteCostCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteCostCategoryHandler> _logger;

        public DeleteCostCategoryHandler(SttbDbContext db, ILogger<DeleteCostCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteCostCategoryResponse> Handle(DeleteCostCategoryRequest request, CancellationToken ct)
        {
            var category = await _db.AcademicProgramCostCategories
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            if (category == null)
            {
                throw new Exception("Data doesnt exist");
            }

            _db.AcademicProgramCostCategories.Remove(category);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully deleted AcademicProgramCostCategory {Id}.", request.Id);

            return new DeleteCostCategoryResponse();
        }
    }
}
