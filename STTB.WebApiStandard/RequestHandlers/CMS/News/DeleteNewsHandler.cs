using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.News
{
    public class DeleteNewsHandler : IRequestHandler<DeleteNewsRequest, DeleteNewsResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteNewsHandler> _logger;

        public DeleteNewsHandler(SttbDbContext db, ILogger<DeleteNewsHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteNewsResponse> Handle(DeleteNewsRequest request, CancellationToken ct)
        {
            var news = await _db.NewsPosts
                .Include(n => n.NewsPostCategories)
                .FirstOrDefaultAsync(n => n.Id == request.Id, ct);

            if (news == null)
            {
                throw new KeyNotFoundException($"News Post with ID {request.Id} was not found.");
            }

            // Remove Maps
            _db.NewsPostCategories.RemoveRange(news.NewsPostCategories);

            // Handle physical image cleanup
            var existingAsset = await _db.Assets
                .FirstOrDefaultAsync(a => a.ModelType == @"news_posts\news_image" && a.ModelId == news.Id, ct);

            if (existingAsset != null)
            {
                var oldPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingAsset.FilePath.TrimStart('/'));
                if (File.Exists(oldPhysicalPath)) File.Delete(oldPhysicalPath);

                _db.Assets.Remove(existingAsset);
            }

            _db.NewsPosts.Remove(news);

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("News Post {Id} deleted successfully.", news.Id);

            return new DeleteNewsResponse();
        }
    }
}
