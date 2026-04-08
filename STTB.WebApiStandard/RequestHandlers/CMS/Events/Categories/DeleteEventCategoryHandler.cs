using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events.Categories
{
    public class DeleteEventCategoryHandler : IRequestHandler<DeleteEventCategoryRequest, DeleteEventCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteEventCategoryHandler> _logger;

        public DeleteEventCategoryHandler(SttbDbContext db, ILogger<DeleteEventCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteEventCategoryResponse> Handle(DeleteEventCategoryRequest request, CancellationToken ct)
        {
            var eventCategory = await _db.EventCategories
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (eventCategory == null)
            {
                throw new Exception("Data doesnt exist");
            }

            _db.EventCategories.Remove(eventCategory);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Successfully deleted EventCategory {Id}.", request.Id);

            return new DeleteEventCategoryResponse();
        }
    }
}
