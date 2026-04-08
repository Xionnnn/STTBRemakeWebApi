using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Events;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events.Categories
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
            if (request.FetchAll)
            {
                var categories = await _db.EventCategories
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .Select(c => new GetAllEventCategoriesDTO
                    {
                        Id = c.Id,
                        CategoryName = c.Name,
                        Slug = c.Slug
                    })
                    .ToListAsync(ct);

                var response = new GetAllEventCategoriesResponse { Items = categories };

                _logger.LogInformation("Retrieved all EventCategories. Total items: {Count}", categories.Count);

                return response;
            }
            else
            {
                var query = _db.EventCategories.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(request.CategoryName))
                {
                    query = query.Where(c => c.Name.Contains(request.CategoryName));
                }

                query = ApplySorting(query, request.OrderBy, request.OrderState);

                var totalItems = await query.CountAsync(ct);
                var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

                var categoryList = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new GetAllEventCategoriesDTO
                    {
                        Id = c.Id,
                        CategoryName = c.Name,
                        Slug = c.Slug
                    })
                    .ToListAsync(ct);

                var response = new GetAllEventCategoriesResponse
                {
                    Items = categoryList,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = totalPages
                };

                _logger.LogInformation("Retrieved EventCategories for page {PageNumber}. Items count: {Count}, Total Pages: {TotalPages}",
                    request.PageNumber, categoryList.Count, totalPages);

                return response;
            }
        }

        private IQueryable<EventCategory> ApplySorting(IQueryable<EventCategory> query, string orderBy, string orderState)
        {
            var isDescending = orderState?.ToLower() == "desc";

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                "CreatedAt" => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                "CategoryName" => isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                "Slug" => isDescending ? query.OrderByDescending(c => c.Slug) : query.OrderBy(c => c.Slug),
                _ => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
            };
        }
    }
}
