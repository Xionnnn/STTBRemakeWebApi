using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionDeadline;
using STTB.WebApiStandard.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionDeadlines
{
    public class EditBatchDeadlineHandler : IRequestHandler<EditBatchDeadlineRequest, EditBatchDeadlineResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditBatchDeadlineHandler> _logger;

        public EditBatchDeadlineHandler(SttbDbContext db, ILogger<EditBatchDeadlineHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditBatchDeadlineResponse> Handle(EditBatchDeadlineRequest request, CancellationToken ct)
        {
            var deadline = await _db.AdmissionDeadlines
                .FirstOrDefaultAsync(d => d.Id == request.Id, ct);

            if (deadline == null)
            {
                throw new KeyNotFoundException($"Batch deadline with ID {request.Id} was not found.");
            }

            deadline.AcademicYear = request.AcademicYear;
            deadline.BatchOrder = request.BatchOrder;
            deadline.BatchDeadlineAt = DateTime.SpecifyKind(request.BatchDeadlineAt, DateTimeKind.Utc);
            deadline.FormReturnDeadlineAt = DateTime.SpecifyKind(request.FormReturnDeadlineAt, DateTimeKind.Utc);
            deadline.DocumentSelectionDeadlineAt = DateTime.SpecifyKind(request.DocumentSelectionDeadlineAt, DateTimeKind.Utc);
            deadline.ResultBroadcastAt = DateTime.SpecifyKind(request.ResultBroadcastAt, DateTimeKind.Utc);
            deadline.ParticipantCallAt = DateTime.SpecifyKind(request.ParticipantCallAt, DateTimeKind.Utc);
            deadline.IsActive = request.IsActive;
            deadline.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Batch deadline {Id} updated successfully.", deadline.Id);

            return new EditBatchDeadlineResponse
            {
                Id = deadline.Id,
                AcademicYear = deadline.AcademicYear,
                BatchOrder = deadline.BatchOrder,
                BatchDeadlineAt = deadline.BatchDeadlineAt,
                FormReturnDeadlineAt = deadline.FormReturnDeadlineAt,
                DocumentSelectionDeadlineAt = deadline.DocumentSelectionDeadlineAt,
                ResultBroadcastAt = deadline.ResultBroadcastAt,
                ParticipantCallAt = deadline.ParticipantCallAt,
                CreatedAt = deadline.CreatedAt,
                IsActive = deadline.IsActive
            };
        }
    }
}
