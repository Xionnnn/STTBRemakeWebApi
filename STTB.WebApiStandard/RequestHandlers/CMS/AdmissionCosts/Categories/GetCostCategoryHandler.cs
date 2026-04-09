using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts.Categories
{
    public class GetCostCategoryHandler : IRequestHandler<GetCostCategoryRequest, GetCostCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetCostCategoryHandler> _logger;

        public GetCostCategoryHandler(SttbDbContext db, ILogger<GetCostCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetCostCategoryResponse> Handle(GetCostCategoryRequest request, CancellationToken ct)
        {
            var category = await _db.AcademicProgramCostCategories
                .AsNoTracking()
                .Select(nc => new GetCostCategoryResponse
                {
                    Id = nc.Id,
                    CategoryName = nc.CategoryName
                })
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            _logger.LogInformation("Retrieved AcademicProgramCostCategory {Id}. Found: {Found}", request.Id, category != null);

            return category!;
        }
    }
}
