using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Admissions;
using STTB.WebApiStandard.Contracts.ResponseModels.Admissions;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Admissions
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
            var schedule = await _db.AdmissionDeadlines
                .AsNoTracking()
                .OrderByDescending(d => d.CreatedAt)
                .FirstOrDefaultAsync(ct);

            if (schedule == null)
            {
                _logger.LogInformation("No admission schedule found.");
                return new GetAdmissionScheduleResponse();
            }

            return new GetAdmissionScheduleResponse
            {
                FirstBatchDeadline = schedule.FirstBatchClosingAt,
                SecondBatchDeadline = schedule.SecondBatchClosingAt,
                ThirdBatchDeadline = schedule.ThirdBatchClosingAt
            };
        }
    }
}
