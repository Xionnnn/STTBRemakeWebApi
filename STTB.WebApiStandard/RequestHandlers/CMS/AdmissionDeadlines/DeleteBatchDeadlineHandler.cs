using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionDeadline;
using STTB.WebApiStandard.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionDeadlines
{
    public class DeleteBatchDeadlineHandler : IRequestHandler<DeleteBatchDeadlineRequest, DeleteBatchDeadlineResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteBatchDeadlineHandler> _logger;

        public DeleteBatchDeadlineHandler(SttbDbContext db, ILogger<DeleteBatchDeadlineHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteBatchDeadlineResponse> Handle(DeleteBatchDeadlineRequest request, CancellationToken ct)
        {
            var deadline = await _db.AdmissionDeadlines
                .FirstOrDefaultAsync(d => d.Id == request.Id, ct);

            if (deadline == null)
            {
                throw new KeyNotFoundException($"Batch deadline with ID {request.Id} was not found.");
            }

            _db.AdmissionDeadlines.Remove(deadline);
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Batch deadline {Id} deleted successfully.", deadline.Id);

            return new DeleteBatchDeadlineResponse();
        }
    }
}
