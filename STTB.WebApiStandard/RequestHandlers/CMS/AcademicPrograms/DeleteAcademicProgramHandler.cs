using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AcademicPrograms
{
    public class DeleteAcademicProgramHandler : IRequestHandler<DeleteAcademicProgramRequest, DeleteAcademicProgramResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteAcademicProgramHandler> _logger;

        public DeleteAcademicProgramHandler(SttbDbContext db, ILogger<DeleteAcademicProgramHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteAcademicProgramResponse> Handle(DeleteAcademicProgramRequest request, CancellationToken ct)
        {
            // Cascade delete handled typically in DB or EF configuration.
            // If explicit cascade is needed we load children and then remove.
            var program = await _db.AcademicPrograms
                .Include(p => p.AcademicProgramRequirements)
                .Include(p => p.AcademicProgramNotes)
                .Include(p => p.AcademicProgramSystems)
                .Include(p => p.AcademicProgramCategories)
                    .ThenInclude(c => c.AcademicCategoryCourses)
                .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (program == null)
            {
                throw new KeyNotFoundException($"Academic Program with ID {request.Id} was not found.");
            }

            // Explicitly remove dependent collections to avoid constraint violations if cascading is off
            _db.AcademicProgramRequirements.RemoveRange(program.AcademicProgramRequirements);
            _db.AcademicProgramNotes.RemoveRange(program.AcademicProgramNotes);
            _db.AcademicProgramSystems.RemoveRange(program.AcademicProgramSystems);

            foreach (var cat in program.AcademicProgramCategories)
            {
                _db.AcademicCategoryCourses.RemoveRange(cat.AcademicCategoryCourses);
            }
            _db.AcademicProgramCategories.RemoveRange(program.AcademicProgramCategories);

            _db.AcademicPrograms.Remove(program);
            
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Academic Program {Id} deleted successfully.", program.Id);

            return new DeleteAcademicProgramResponse();
        }
    }
}
