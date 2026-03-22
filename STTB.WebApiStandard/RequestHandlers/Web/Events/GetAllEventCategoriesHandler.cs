using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.Events;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Web.Events
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

            _logger.LogInformation($"Found {categories.Count} event categories");

            return new GetAllEventCategoriesResponse
            {
                Items = categories
            };
        }
    }
}
