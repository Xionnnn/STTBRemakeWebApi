using MediatR;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Dashboard;
using STTB.WebApiStandard.Contracts.ResponseModels.Dashboards;
using STTB.WebApiStandard.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace STTB.WebApiStandard.RequestHandlers.CMS.Dashboards
{
    public class GetDashboardDataHandler : IRequestHandler<GetDashboardDataRequest, GetDashboardDataResponse>
    {
        private readonly SttbDbContext _db;

        public GetDashboardDataHandler(SttbDbContext db)
        {
            _db = db;
        }

        public async Task<GetDashboardDataResponse> Handle(GetDashboardDataRequest request, CancellationToken ct)
        {
            return new GetDashboardDataResponse
            {
                TotalNews = await _db.NewsPosts.CountAsync(ct),
                TotalEvent = await _db.Events.CountAsync(ct),
                TotalAcademicProgram = await _db.AcademicPrograms.CountAsync(ct),
                TotalLecturer = await _db.Lecturers.CountAsync(ct),
                TotalAdministrator = await _db.FoundationAdministrators.CountAsync(ct),
                TotalVideo = await _db.MediaItems.CountAsync(m => m.MediaFormat == "video", ct),
                TotalArticle = await _db.MediaItems.CountAsync(m => m.MediaFormat == "article", ct),
                TotalJournal = await _db.MediaItems.CountAsync(m => m.MediaFormat == "journal", ct),
                TotalMonograf = await _db.MediaItems.CountAsync(m => m.MediaFormat == "monograf", ct),
                TotalBuletin = await _db.MediaItems.CountAsync(m => m.MediaFormat == "buletin", ct)
            };
        }
    }
}
