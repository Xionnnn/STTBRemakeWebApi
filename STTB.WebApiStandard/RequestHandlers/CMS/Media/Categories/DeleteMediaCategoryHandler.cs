using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media.Categories
{
    public class DeleteMediaCategoryHandler : IRequestHandler<DeleteMediaCategoryRequest, DeleteMediaCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteMediaCategoryHandler> _logger;

        public DeleteMediaCategoryHandler(SttbDbContext db, ILogger<DeleteMediaCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteMediaCategoryResponse> Handle(DeleteMediaCategoryRequest request, CancellationToken ct)
        {
            var category = await _db.MediaTopicCategories
                .FirstOrDefaultAsync(nc => nc.Id == request.Id, ct);

            if (category == null)
            {
                throw new Exception("Data doesnt exist");
            }

            _db.MediaTopicCategories.Remove(category);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully deleted MediaCategory {Id}.", request.Id);

            return new DeleteMediaCategoryResponse();
        }
    }
}
