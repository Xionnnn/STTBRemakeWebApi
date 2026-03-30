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
    public class GetBatchDeadlineHandler : IRequestHandler<GetBatchDeadlineRequest, GetBatchDeadlineResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetBatchDeadlineHandler> _logger;

        public GetBatchDeadlineHandler(SttbDbContext db, ILogger<GetBatchDeadlineHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetBatchDeadlineResponse> Handle(GetBatchDeadlineRequest request, CancellationToken ct)
        {
            var deadline = await _db.AdmissionDeadlines
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == request.Id, ct);

            if (deadline == null)
            {
                throw new KeyNotFoundException($"Batch deadline with ID {request.Id} was not found.");
            }

            _logger.LogInformation("Fetched batch deadline {Id}.", deadline.Id);

            return new GetBatchDeadlineResponse
            {
                Id = deadline.Id,
                AcademicYear = deadline.AcademicYear,
                BatchOrder = deadline.BatchOrder,
                BatchDeadlineAt = deadline.BatchDeadlineAt,
                FormReturnDeadlineAt = deadline.FormReturnDeadlineAt,
                DocumentSelectionDeadlineAt = deadline.DocumentSelectionDeadlineAt,
                ResultBroadcastAt = deadline.ResultBroadcastAt,
                ParticipantCallAt = deadline.ParticipantCallAt,
                IsActive = deadline.IsActive
            };
        }
    }
}
