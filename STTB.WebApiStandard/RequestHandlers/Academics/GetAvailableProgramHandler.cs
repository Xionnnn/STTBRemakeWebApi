using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Academic;
using STTB.WebApiStandard.Contracts.ResponseModels.Academic;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Academics
{
    public class GetAvailableProgramHandler : IRequestHandler<GetAvailableProgramRequest, GetAvailableProgramResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAvailableProgramHandler> _logger;
        public GetAvailableProgramHandler(SttbDbContext db, ILogger<GetAvailableProgramHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAvailableProgramResponse> Handle(GetAvailableProgramRequest request, CancellationToken ct)
        {
            var query = _db.AcademicPrograms
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking();

            if (request.FetchLimit.HasValue)
            {
                query = query.Take(request.FetchLimit.Value);
            }

            var items = await query
                .Select(p => new ItemDTO
                {
                    ProgramId = p.Id,
                    ProgramName = p.Name,
                    Degree = p.DegreeAbbr,
                    Duration = p.StudyDuration,
                    TotalCredit = p.TotalCredits
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} programs");

            return new GetAvailableProgramResponse
            {
                Items = items
            };
        }
    }
}
