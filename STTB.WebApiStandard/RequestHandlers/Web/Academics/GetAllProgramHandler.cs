using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.DTOs.Web.Academics;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Academics;
using STTB.WebApiStandard.Contracts.ResponseModels.Academic;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Web.Academics
{
    public class GetAllProgramHandler : IRequestHandler<GetAllAcademicProgramRequest, GetAllAcademicProgramResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllProgramHandler> _logger;
        public GetAllProgramHandler(SttbDbContext db, ILogger<GetAllProgramHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllAcademicProgramResponse> Handle(GetAllAcademicProgramRequest request, CancellationToken ct)
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
                .Select(p => new GetAllAcademicDTO
                {
                    ProgramId = p.Id,
                    Slug = p.Slug,
                    ProgramName = p.Name,
                    Degree = p.DegreeAbbr,
                    Duration = p.StudyDuration,
                    TotalCredit = p.TotalCredits
                })
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} programs");

            return new GetAllAcademicProgramResponse
            {
                Items = items
            };
        }
    }
}
