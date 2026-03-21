using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Academics;
using STTB.WebApiStandard.Contracts.ResponseModels.Academics;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Academics
{
    public class GetAcademicRequirementsHandler : IRequestHandler<GetAcademicRequirementsRequest, GetAcademicRequirementsResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAcademicRequirementsHandler> _logger;

        public GetAcademicRequirementsHandler(SttbDbContext db, ILogger<GetAcademicRequirementsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAcademicRequirementsResponse> Handle(GetAcademicRequirementsRequest request, CancellationToken ct)
        {
            var items = await _db.AcademicPrograms
                .Include(p => p.AcademicProgramRequirements)
                .Where(p => p.IsPublished)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Select(p => new ProgramRequirementDto
                {
                    ProgramName = p.Name,
                    Degree = p.DegreeAbbr,
                    Requirements = p.AcademicProgramRequirements
                        .OrderBy(r => r.Id)
                        .Select(r => r.RequirementText)
                        .ToList()
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found requirements for {items.Count} academic programs");

            return new GetAcademicRequirementsResponse
            {
                Items = items
            };
        }
    }
}
