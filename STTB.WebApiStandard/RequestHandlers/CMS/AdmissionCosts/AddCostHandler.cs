using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts
{
    public class AddCostHandler : IRequestHandler<AddCostRequest, AddCostResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<AddCostHandler> _logger;

        public AddCostHandler(SttbDbContext db, ILogger<AddCostHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<AddCostResponse> Handle(AddCostRequest request, CancellationToken ct)
        {
            // Verify Program
            var program = await _db.AcademicPrograms.FirstOrDefaultAsync(p => p.Name == request.ProgramName, ct);
            if (program == null)
                throw new InvalidOperationException($"Academic Program '{request.ProgramName}' not found.");

            // Verify Category
            var category = await _db.AcademicProgramCostCategories.FirstOrDefaultAsync(c => c.CategoryName == request.CategoryName, ct);
            if (category == null)
                throw new InvalidOperationException($"Category '{request.CategoryName}' not found.");

            var cost = new AcademicProgramCost
            {
                Name = request.CostName,
                Price = request.Cost,
                AcademicProgramId = program.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.AcademicProgramCosts.AddAsync(cost, ct);
            await _db.SaveChangesAsync(ct);

            // Rebuild Category Map
            cost.AcademicProgramCostCategoryMaps.Add(new AcademicProgramCostCategoryMap
            {
                AcademicProgramCostId = cost.Id,
                AcademicProgramCostCategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Cost {Id} created successfully.", cost.Id);

            return new AddCostResponse
            {
                Id = cost.Id,
                CategoryName = category.CategoryName,
                ProgramName = program.Name,
                CostName = cost.Name,
                Cost = cost.Price
            };
        }
    }
}
