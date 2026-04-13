using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Contracts.DTOs.CMS.AdmissionCosts;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts
{
    public class GetAllCostHandler : IRequestHandler<GetAllCostRequest, GetAllCostResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllCostHandler> _logger;

        public GetAllCostHandler(SttbDbContext db, ILogger<GetAllCostHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllCostResponse> Handle(GetAllCostRequest request, CancellationToken ct)
        {
            var query = _db.AcademicProgramCosts
                .Include(c => c.AcademicProgram)
                .Include(c => c.AcademicProgramCostCategoryMaps)
                    .ThenInclude(m => m.AcademicProgramCostCategory)
                .AsNoTracking();

            // Filter by name
            if (!string.IsNullOrWhiteSpace(request.CostName))
            {
                query = query.Where(c => c.Name.Contains(request.CostName));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CostDTO
                {
                    Id = c.Id,
                    CategoryName = c.AcademicProgramCostCategoryMaps.FirstOrDefault() != null 
                        ? c.AcademicProgramCostCategoryMaps.First().AcademicProgramCostCategory.CategoryName 
                        : string.Empty,
                    ProgramName = c.AcademicProgram.Name,
                    CostName = c.Name,
                    Cost = c.Price,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(ct);

            _logger.LogInformation("Found {Count} costs out of {Total} total.", items.Count, totalItems);

            return new GetAllCostResponse
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<AcademicProgramCost> ApplySorting(
            IQueryable<AcademicProgramCost> query,
            string orderBy,
            string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderByDescending(c => c.CreatedAt);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending
                    ? query.OrderByDescending(c => c.Id)
                    : query.OrderBy(c => c.Id),

                "CostName" => isDescending
                    ? query.OrderByDescending(c => c.Name)
                    : query.OrderBy(c => c.Name),

                "Cost" => isDescending
                    ? query.OrderByDescending(c => c.Price)
                    : query.OrderBy(c => c.Price),

                "ProgramName" => isDescending
                    ? query.OrderByDescending(c => c.AcademicProgram.Name)
                    : query.OrderBy(c => c.AcademicProgram.Name),
                    
                // For CategoryName sorting, we'd have to sort by the first mapped category name
                "CategoryName" => isDescending
                    ? query.OrderByDescending(c => c.AcademicProgramCostCategoryMaps.FirstOrDefault()!.AcademicProgramCostCategory.CategoryName)
                    : query.OrderBy(c => c.AcademicProgramCostCategoryMaps.FirstOrDefault()!.AcademicProgramCostCategory.CategoryName),

                "CreatedAt" => isDescending
                    ? query.OrderByDescending(c => c.CreatedAt)
                    : query.OrderBy(c => c.CreatedAt),

                _ => query.OrderByDescending(c => c.CreatedAt)
            };
        }
    }
}
