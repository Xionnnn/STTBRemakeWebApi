using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Admissions;
using STTB.WebApiStandard.Contracts.ResponseModels.Admissions;
using STTB.WebApiStandard.Contracts.DTOs.Web.Admissions;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Web.Admissions
{
    public class GetAdmissionScheduleHandler : IRequestHandler<GetAdmissionScheduleRequest, GetAdmissionScheduleResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAdmissionScheduleHandler> _logger;

        public GetAdmissionScheduleHandler(SttbDbContext db, ILogger<GetAdmissionScheduleHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAdmissionScheduleResponse> Handle(GetAdmissionScheduleRequest request, CancellationToken ct)
        {
            var schedules = await _db.AdmissionDeadlines
                .AsNoTracking()
                .Where(d => d.IsActive)
                .OrderBy(d => d.BatchOrder)
                .Select(d => new AdmissionScheduleDTO
                {
                    AcademicYear = d.AcademicYear,
                    BatchOrder = d.BatchOrder,
                    BatchDeadlineAt = d.BatchDeadlineAt,
                    FormReturnDeadlineAt = d.FormReturnDeadlineAt,
                    DocumentSelectionDeadlineAt = d.DocumentSelectionDeadlineAt,
                    ResultBroadcastAt = d.ResultBroadcastAt,
                    ParticipantCallAt = d.ParticipantCallAt
                })
                .ToListAsync(ct);

            _logger.LogInformation("Found {Count} admission schedules.", schedules.Count);

            return new GetAdmissionScheduleResponse
            {
                Items = schedules
            };
        }
    }
}
