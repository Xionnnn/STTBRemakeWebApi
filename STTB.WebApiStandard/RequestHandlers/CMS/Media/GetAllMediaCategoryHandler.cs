using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media;
using STTB.WebApiStandard.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Media
{
    public class GetAllMediaCategoryHandler : IRequestHandler<GetAllMediaCategoryRequest, GetAllMediaCategoryResponse>
    {
        private readonly SttbDbContext _db;

        public GetAllMediaCategoryHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetAllMediaCategoryResponse> Handle(GetAllMediaCategoryRequest request, CancellationToken ct)
        {
            var categories = await _db.MediaTopicCategories
                .Select(c => c.Name)
                .OrderBy(n => n)
                .ToListAsync(ct);

            return new GetAllMediaCategoryResponse
            {
                Categories = categories
            };
        }
    }
}
