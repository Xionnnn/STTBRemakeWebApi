using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Categories
{
    public class GetAllMediaCategoryHandler : IRequestHandler<GetAllMediaCategoryRequest, GetAllMediaCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllMediaCategoryHandler> _logger;

        public GetAllMediaCategoryHandler(SttbDbContext db, ILogger<GetAllMediaCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllMediaCategoryResponse> Handle(GetAllMediaCategoryRequest request, CancellationToken ct)
        {
            if (request.FetchAll)
            {
                var categories = await _db.MediaTopicCategories
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .Select(c => new MediaCategoryDTO
                    {
                        Id = c.Id,
                        CategoryName = c.Name,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync(ct);

                var response = new GetAllMediaCategoryResponse { Categories = categories };

                _logger.LogInformation("Retrieved all MediaCategories. Total items: {Count}", categories.Count);

                return response;
            }
            else
            {
                var query = _db.MediaTopicCategories.AsNoTracking();

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
                    .Select(c => new MediaCategoryDTO
                    {
                        Id = c.Id,
                        CategoryName = c.Name,
                        CreatedAt = c.CreatedAt
                    })
                    .ToListAsync(ct);

                var response = new GetAllMediaCategoryResponse
                {
                    Categories = categoryList,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    TotalMedia = totalItems
                };

                _logger.LogInformation("Retrieved MediaCategories for page {PageNumber}. Items count: {Count}, Total Pages: {TotalPages}",
                    request.PageNumber, categoryList.Count, totalPages);

                return response;
            }
        }

        private IQueryable<MediaTopicCategory> ApplySorting(IQueryable<MediaTopicCategory> query, string orderBy, string orderState)
        {
            var isDescending = orderState?.ToLower() == "desc";

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                "CategoryName" => isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                "CreatedAt" => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                _ => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
            };
        }
    }
}
