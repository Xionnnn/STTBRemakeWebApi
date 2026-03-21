using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.Events;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.Events
{
    public class GetAllOrganizersHandler : IRequestHandler<GetAllOrganizersRequest, GetAllOrganizersResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllOrganizersHandler> _logger;
        public GetAllOrganizersHandler(SttbDbContext db, ILogger<GetAllOrganizersHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllOrganizersResponse> Handle(GetAllOrganizersRequest request, CancellationToken ct)
        {
            var items = await _db.EventOrganizers
                .AsNoTracking()
                .OrderBy(o => o.Name)
                .Select(o => o.Name)
                .ToListAsync(ct);

            _logger.LogInformation($"Found {items.Count} organizers");

            return new GetAllOrganizersResponse
            {
                Items = items
            };
        }
    }
}
