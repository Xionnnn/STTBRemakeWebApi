using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News.Categories
{
    public class DeleteNewsCategoryHandler : IRequestHandler<DeleteNewsCategoryRequest, DeleteNewsCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteNewsCategoryHandler> _logger;

        public DeleteNewsCategoryHandler(SttbDbContext db, ILogger<DeleteNewsCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteNewsCategoryResponse> Handle(DeleteNewsCategoryRequest request, CancellationToken ct)
        {
            var newsCategory = await _db.NewsCategories
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            if (newsCategory == null)
            {
                throw new Exception("Data doesnt exist");
            }

            _db.NewsCategories.Remove(newsCategory);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully deleted NewsCategory {Id}.", request.Id);

            return new DeleteNewsCategoryResponse();
        }
    }
}
