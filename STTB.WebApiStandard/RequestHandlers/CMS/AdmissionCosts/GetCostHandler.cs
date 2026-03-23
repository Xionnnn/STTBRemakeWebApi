using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts
{
    public class GetCostHandler : IRequestHandler<GetCostRequest, GetCostResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetCostHandler> _logger;

        public GetCostHandler(SttbDbContext db, ILogger<GetCostHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetCostResponse> Handle(GetCostRequest request, CancellationToken ct)
        {
            var cost = await _db.AcademicProgramCosts
                .Include(c => c.AcademicProgram)
                .Include(c => c.AcademicProgramCostCategoryMaps)
                    .ThenInclude(m => m.AcademicProgramCostCategory)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (cost == null)
            {
                throw new KeyNotFoundException($"Cost with ID {request.Id} was not found.");
            }

            return new GetCostResponse
            {
                Id = cost.Id,
                CategoryName = cost.AcademicProgramCostCategoryMaps.FirstOrDefault() != null 
                    ? cost.AcademicProgramCostCategoryMaps.First().AcademicProgramCostCategory.CategoryName 
                    : string.Empty,
                ProgramName = cost.AcademicProgram?.Name ?? string.Empty,
                CostName = cost.Name,
                Cost = cost.Price
            };
        }
    }
}
