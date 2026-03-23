using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events
{
    public class GetAllEventCategoriesHandler : IRequestHandler<GetAllEventCategoriesRequest, GetAllEventCategoriesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllEventCategoriesHandler> _logger;

        public GetAllEventCategoriesHandler(SttbDbContext db, ILogger<GetAllEventCategoriesHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllEventCategoriesResponse> Handle(GetAllEventCategoriesRequest request, CancellationToken ct)
        {
            var categories = await _db.EventCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToListAsync(ct);

            _logger.LogInformation($"Found {categories.Count} event categories for CMS.");

            return new GetAllEventCategoriesResponse
            {
                Items = categories
            };
        }
    }
}
