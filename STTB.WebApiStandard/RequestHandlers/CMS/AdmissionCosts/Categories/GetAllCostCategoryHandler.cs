using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Contracts.DTOs.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts.Categories
{
    public class GetAllCostCategoryHandler : IRequestHandler<GetAllCostCategoryRequest, GetAllCostCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllCostCategoryHandler> _logger;

        public GetAllCostCategoryHandler(SttbDbContext db, ILogger<GetAllCostCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllCostCategoryResponse> Handle(GetAllCostCategoryRequest request, CancellationToken ct)
        {
            if (request.FetchAll)
            {
                var categories = await _db.AcademicProgramCostCategories
                    .AsNoTracking()
                    .OrderBy(c => c.CategoryName)
                    .Select(c => new CMSCostCategoryDTO
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync(ct);

                var response = new GetAllCostCategoryResponse { Items = categories };

                _logger.LogInformation("Retrieved all AcademicProgramCostCategories. Total items: {Count}", categories.Count);

                return response;
            }
            else
            {
                var query = _db.AcademicProgramCostCategories.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(request.CategoryName))
                {
                    query = query.Where(c => c.CategoryName.ToLower().Contains(request.CategoryName.ToLower()));
                }

                query = ApplySorting(query, request.OrderBy, request.OrderState);

                var totalItems = await query.CountAsync(ct);
                var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);
                
                var categoryList = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new CMSCostCategoryDTO
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync(ct);

                var response = new GetAllCostCategoryResponse
                {
                    Items = categoryList,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    TotalItems = totalItems
                };

                _logger.LogInformation("Retrieved AcademicProgramCostCategories for page {PageNumber}. Items count: {Count}, Total Pages: {TotalPages}",
                    request.PageNumber, categoryList.Count, totalPages);

                return response;
            }
        }

        private IQueryable<AcademicProgramCostCategory> ApplySorting(IQueryable<AcademicProgramCostCategory> query, string orderBy, string orderState)
        {
            var isDescending = orderState?.ToLower() == "desc";

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                "CategoryName" => isDescending ? query.OrderByDescending(c => c.CategoryName) : query.OrderBy(c => c.CategoryName),
                "CreatedAt" => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                _ => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
            };
        }
    }
}
