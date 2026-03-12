using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.Media
{
    public class GetMediaCategoriesHandler : IRequestHandler<GetMediaCategoriesRequest, GetMediaCategoriesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetMediaCategoriesHandler> _logger;

        public GetMediaCategoriesHandler(SttbDbContext db, ILogger<GetMediaCategoriesHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetMediaCategoriesResponse> Handle(GetMediaCategoriesRequest request, CancellationToken ct)
        {
            var categories = await _db.MediaTopicCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToListAsync(ct);

            _logger.LogInformation($"Found {categories.Count} media categories");

            return new GetMediaCategoriesResponse
            {
                Items = categories
            };
        }
    }
}
