using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Categories
{
    public class GetMediaCategoryHandler : IRequestHandler<GetMediaCategoryRequest, GetMediaCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetMediaCategoryHandler> _logger;

        public GetMediaCategoryHandler(SttbDbContext db, ILogger<GetMediaCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetMediaCategoryResponse> Handle(GetMediaCategoryRequest request, CancellationToken ct)
        {
            var category = await _db.MediaTopicCategories
                .AsNoTracking()
                .Select(nc => new GetMediaCategoryResponse
                {
                    Id = nc.Id,
                    CategoryName = nc.Name
                })
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            _logger.LogInformation("Retrieved MediaCategory {Id}. Found: {Found}", request.Id, category != null);

            return category!;
        }
    }
}
