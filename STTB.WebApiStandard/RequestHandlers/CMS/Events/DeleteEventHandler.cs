using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Events
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventRequest, DeleteEventResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<DeleteEventHandler> _logger;

        public DeleteEventHandler(SttbDbContext db, ILogger<DeleteEventHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DeleteEventResponse> Handle(DeleteEventRequest request, CancellationToken ct)
        {
            var ev = await _db.Events
                .Include(e => e.EventCategoryMaps)
                .FirstOrDefaultAsync(e => e.Id == request.Id, ct);

            if (ev == null)
            {
                throw new KeyNotFoundException($"Event with ID {request.Id} was not found.");
            }

            // Remove Maps
            _db.EventCategoryMaps.RemoveRange(ev.EventCategoryMaps);

            // Handle physical image cleanup
            var existingAsset = await _db.Assets
                .FirstOrDefaultAsync(a => a.ModelType == @"events\event_image" && a.ModelId == ev.Id, ct);

            if (existingAsset != null)
            {
                var oldPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingAsset.FilePath.TrimStart('/'));
                if (File.Exists(oldPhysicalPath)) File.Delete(oldPhysicalPath);

                _db.Assets.Remove(existingAsset);
            }

            _db.Events.Remove(ev);

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Event {Id} deleted successfully.", ev.Id);

            return new DeleteEventResponse();
        }
    }
}
