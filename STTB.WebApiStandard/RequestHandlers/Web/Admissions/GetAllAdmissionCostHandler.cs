using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Admissions;
using STTB.WebApiStandard.Contracts.ResponseModels.Admissions;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Web.Admissions
{
    public class GetAllAdmissionCostHandler : IRequestHandler<GetAllAdmissionCostRequest, GetAllAdmissionCostResponse>
    {
        private readonly SttbDbContext _db;

        public GetAllAdmissionCostHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetAllAdmissionCostResponse> Handle(GetAllAdmissionCostRequest request, CancellationToken ct)
        {
            var programs = await _db.AcademicPrograms
                .Where(p => p.IsPublished)
                .OrderBy(p => p.Name)
                .Select(p => new ProgramCostDTO
                {
                    Id = (int)p.Id,
                    ProgramName = p.Name,
                    Slug = p.Slug,
                    CostCategory = _db.AcademicProgramCostCategories
                        .Where(c => c.AcademicProgramCostCategoryMaps.Any(m => m.AcademicProgramCost.AcademicProgramId == p.Id))
                        .OrderBy(c => c.Id)
                        .Select(c => new CategoryCostDTO
                        {
                            CategoryName = c.CategoryName,
                            CostBreakdown = c.AcademicProgramCostCategoryMaps
                                .Where(m => m.AcademicProgramCost.AcademicProgramId == p.Id)
                                .Select(m => new IndividualCostDTO
                                {
                                    CostName = m.AcademicProgramCost.Name,
                                    Cost = m.AcademicProgramCost.Price
                                })
                                .OrderBy(cost => cost.CostName)
                                .ToList()
                        })
                        .ToList()
                })
                .AsNoTracking()
                .ToListAsync(ct);

            return new GetAllAdmissionCostResponse
            {
                Items = programs
            };
        }
    }
}
