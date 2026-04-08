using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News.Categories
{
    public class GetNewsCategoryHandler : IRequestHandler<GetNewsCategoryRequest, GetNewsCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetNewsCategoryHandler> _logger;

        public GetNewsCategoryHandler(SttbDbContext db, ILogger<GetNewsCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetNewsCategoryResponse> Handle(GetNewsCategoryRequest request, CancellationToken ct)
        {
            var newsCategory = await _db.NewsCategories
                .Select(nc => new GetNewsCategoryResponse
                {
                    Id = nc.Id,
                    CategoryName = nc.Name
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            _logger.LogInformation("Retrieved NewsCategory {Id}. Found: {Found}", request.Id, newsCategory != null);

            return newsCategory!;
        }
    }
}
