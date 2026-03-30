using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionDeadline;
using STTB.WebApiStandard.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionDeadlines
{
    public class GetAllBatchDeadlineHandler : IRequestHandler<GetAllBatchDeadlineRequest, GetAllBatchDeadlineResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllBatchDeadlineHandler> _logger;

        public GetAllBatchDeadlineHandler(SttbDbContext db, ILogger<GetAllBatchDeadlineHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllBatchDeadlineResponse> Handle(GetAllBatchDeadlineRequest request, CancellationToken ct)
        {
            var query = _db.AdmissionDeadlines.AsNoTracking();

            // Filter by academic year or batch order
            if (!string.IsNullOrWhiteSpace(request.SearchBatch))
            {
                query = query.Where(d => d.AcademicYear.Contains(request.SearchBatch)
                    || d.BatchOrder.ToString().Contains(request.SearchBatch));
            }

            // Sorting
            query = ApplySorting(query, request.OrderBy, request.OrderState);

            var totalItems = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(d => new CMSGetAllBatchDeadlineDTO
                {
                    Id = d.Id,
                    AcademicYear = d.AcademicYear,
                    BatchOrder = d.BatchOrder,
                    BatchDeadlineAt = d.BatchDeadlineAt,
                    FormReturnDeadlineAt = d.FormReturnDeadlineAt,
                    DocumentSelectionDeadlineAt = d.DocumentSelectionDeadlineAt,
                    ResultBroadcastAt = d.ResultBroadcastAt,
                    ParticipantCallAt = d.ParticipantCallAt,
                    CreatedAt = d.CreatedAt,
                    IsActive = d.IsActive
                })
                .ToListAsync(ct);

            _logger.LogInformation("Found {Count} batch deadlines out of {Total} total.", items.Count, totalItems);

            return new GetAllBatchDeadlineResponse
            {
                Items = items
            };
        }

        private IQueryable<Entities.AdmissionDeadline> ApplySorting(IQueryable<Entities.AdmissionDeadline> query, string orderBy, string orderState)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return query.OrderBy(d => d.BatchOrder);
            }

            var isDescending = orderState.Equals("desc", StringComparison.OrdinalIgnoreCase);

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(d => d.Id) : query.OrderBy(d => d.Id),
                "AcademicYear" => isDescending ? query.OrderByDescending(d => d.AcademicYear) : query.OrderBy(d => d.AcademicYear),
                "BatchOrder" => isDescending ? query.OrderByDescending(d => d.BatchOrder) : query.OrderBy(d => d.BatchOrder),
                "BatchDeadlineAt" => isDescending ? query.OrderByDescending(d => d.BatchDeadlineAt) : query.OrderBy(d => d.BatchDeadlineAt),
                "IsActive" => isDescending ? query.OrderByDescending(d => d.IsActive) : query.OrderBy(d => d.IsActive),
                "CreatedAt" => isDescending ? query.OrderByDescending(d => d.CreatedAt) : query.OrderBy(d => d.CreatedAt),
                _ => query.OrderBy(d => d.BatchOrder)
            };
        }
    }
}
