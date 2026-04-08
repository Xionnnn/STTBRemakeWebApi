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
    public class EditNewsCategoryHandler : IRequestHandler<EditNewsCategoryRequest, EditNewsCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<EditNewsCategoryHandler> _logger;

        public EditNewsCategoryHandler(SttbDbContext db, ILogger<EditNewsCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<EditNewsCategoryResponse> Handle(EditNewsCategoryRequest request, CancellationToken ct)
        {
            var newsCategory = await _db.NewsCategories
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            if (newsCategory == null)
            {
                throw new Exception("Data doesnt exist");
            }

            newsCategory.Name = request.CategoryName;
            newsCategory.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully updated NewsCategory {Id}.", request.Id);

            return new EditNewsCategoryResponse
            {
                Id = request.Id,
                CategoryName = request.CategoryName
            };
        }
    }
}
