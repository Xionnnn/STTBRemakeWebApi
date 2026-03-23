using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News
{
    public class GetAllNewsCategoriesHandler : IRequestHandler<GetAllNewsCategoriesRequest, GetAllNewsCategoriesResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllNewsCategoriesHandler> _logger;

        public GetAllNewsCategoriesHandler(SttbDbContext db, ILogger<GetAllNewsCategoriesHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllNewsCategoriesResponse> Handle(GetAllNewsCategoriesRequest request, CancellationToken ct)
        {
            var categories = await _db.NewsCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToListAsync(ct);

            _logger.LogInformation($"Found {categories.Count} news categories for CMS.");

            return new GetAllNewsCategoriesResponse
            {
                Items = categories
            };
        }
    }
}
