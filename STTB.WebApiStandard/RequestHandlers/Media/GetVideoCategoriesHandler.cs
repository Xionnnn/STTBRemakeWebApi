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
    public class GetVideoCategoriesHandler : IRequestHandler<GetVideoCategoriesRequest, GetVideoCategoriesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetVideoCategoriesHandler> _logger;

        public GetVideoCategoriesHandler(SttbDbContext db, ILogger<GetVideoCategoriesHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetVideoCategoriesResponse> Handle(GetVideoCategoriesRequest request, CancellationToken ct)
        {
            // The prompt says "all video category". Using MediaTopicCategory as video categories.
            var categories = await _db.MediaTopicCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToListAsync(ct);

            _logger.LogInformation($"Found {categories.Count} video categories");

            return new GetVideoCategoriesResponse
            {
                Items = categories
            };
        }
    }
}
