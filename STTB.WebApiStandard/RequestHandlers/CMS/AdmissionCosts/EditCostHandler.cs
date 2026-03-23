using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts
{
    public class EditCostHandler : IRequestHandler<EditCostRequest, EditCostResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditCostHandler> _logger;

        public EditCostHandler(SttbDbContext db, ILogger<EditCostHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditCostResponse> Handle(EditCostRequest request, CancellationToken ct)
        {
            var cost = await _db.AcademicProgramCosts
                .Include(c => c.AcademicProgram)
                .Include(c => c.AcademicProgramCostCategoryMaps)
                    .ThenInclude(m => m.AcademicProgramCostCategory)
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (cost == null)
            {
                throw new KeyNotFoundException($"Cost with ID {request.Id} was not found.");
            }

            // Verify Program
            var program = await _db.AcademicPrograms.FirstOrDefaultAsync(p => p.ProgramName == request.ProgramName, ct);
            if (program == null)
                throw new InvalidOperationException($"Academic Program '{request.ProgramName}' not found.");

            // Verify Category
            var category = await _db.AcademicProgramCostCategories.FirstOrDefaultAsync(c => c.CategoryName == request.CategoryName, ct);
            if (category == null)
                throw new InvalidOperationException($"Category '{request.CategoryName}' not found.");

            // Update basic fields
            cost.Name = request.CostName;
            cost.Price = request.Cost;
            cost.AcademicProgramId = program.Id;
            cost.UpdatedAt = DateTime.UtcNow;

            // Rebuild Category Map
            _db.AcademicProgramCostCategoryMaps.RemoveRange(cost.AcademicProgramCostCategoryMaps);
            cost.AcademicProgramCostCategoryMaps.Add(new AcademicProgramCostCategoryMap
            {
                AcademicProgramCostId = cost.Id,
                AcademicProgramCostCategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Cost {Id} updated successfully.", cost.Id);

            return new EditCostResponse
            {
                Id = cost.Id,
                CategoryName = category.CategoryName,
                ProgramName = program.ProgramName,
                CostName = cost.Name,
                Cost = cost.Price
            };
        }
    }
}
