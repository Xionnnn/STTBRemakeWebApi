using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts
{
    public class DeleteCostHandler : IRequestHandler<DeleteCostRequest, DeleteCostResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteCostHandler> _logger;

        public DeleteCostHandler(SttbDbContext db, ILogger<DeleteCostHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteCostResponse> Handle(DeleteCostRequest request, CancellationToken ct)
        {
            var cost = await _db.AcademicProgramCosts
                .Include(c => c.AcademicProgramCostCategoryMaps)
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (cost == null)
            {
                throw new KeyNotFoundException($"Cost with ID {request.Id} was not found.");
            }

            if (cost.AcademicProgramCostCategoryMaps.Any())
            {
                _db.AcademicProgramCostCategoryMaps.RemoveRange(cost.AcademicProgramCostCategoryMaps);
            }

            _db.AcademicProgramCosts.Remove(cost);
            
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Cost {Id} deleted successfully.", cost.Id);

            return new DeleteCostResponse();
        }
    }
}
