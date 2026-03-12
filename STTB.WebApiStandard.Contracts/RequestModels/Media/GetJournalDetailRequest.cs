using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;

namespace STTB.WebApiStandard.Contracts.RequestModels.Media
{
    public class GetJournalDetailRequest : IRequest<GetJournalDetailResponse>
    {
        public string JournalSlug { get; set; } = string.Empty;
    }
}
