using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events.Categories
{
    public class GetEventCategoryHandler : IRequestHandler<GetEventCategoryRequest, GetEventCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetEventCategoryHandler> _logger;

        public GetEventCategoryHandler(SttbDbContext db, ILogger<GetEventCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetEventCategoryResponse> Handle(GetEventCategoryRequest request, CancellationToken ct)
        {
            var eventCategory = await _db.EventCategories
                .AsNoTracking()
                .Select(c => new GetEventCategoryResponse
                {
                    Id = c.Id,
                    CategoryName = c.Name
                })
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            _logger.LogInformation("Retrieved EventCategory {Id}. Found: {Found}", request.Id, eventCategory != null);

            return eventCategory!;
        }
    }
}
