using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Users.News;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News.Categories
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
            if(request.FetchAll == true)
            {
                var categories = await _db.NewsCategories
               .AsNoTracking()
               .OrderBy(c => c.Name)
               .Select(c => new GetAllNewsCategoriesDTO
               {
                   Id = c.Id,
                   CategoryName = c.Name
               })
               .ToListAsync(ct);

                return new GetAllNewsCategoriesResponse
                {
                    Items = categories
                };
            }
            else
            {
                var query = _db.NewsCategories
                    .AsNoTracking();

                //searching
                if(!string.IsNullOrWhiteSpace(request.CategoryName))
                {
                    query = query.Where(nc => nc.Name == request.CategoryName);
                }

                //orderBy
                ApplySorting(query, request.OrderBy, request.OrderState);

                //pagination
                var totalItems = await query.CountAsync(ct);
                var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

                //fetch list based on the pagination
                var categoryList = await query
                    .Skip((request.PageSize - 1) * request.PageNumber)
                    .Take(request.PageSize)
                    .Select(c => new GetAllNewsCategoriesDTO
                    {
                        Id = c.Id,
                        CategoryName = c.Name
                    })
                    .ToListAsync();

                return new GetAllNewsCategoriesResponse
                {
                    Items = categoryList,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalPages = totalPages
                };
            }
        }

        private IQueryable ApplySorting(IQueryable<NewsCategory> query, string orderBy, string orderState)
        {
            var isDescending = orderState == "Descending" ? true : false;

            return orderBy switch
            {
                "Id" => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                "CreatedAt" => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.Id),
                "CategoryName" => isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                _ => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
            };
        }
    }
}
