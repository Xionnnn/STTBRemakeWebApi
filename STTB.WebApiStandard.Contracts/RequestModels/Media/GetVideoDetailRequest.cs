using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;

namespace STTB.WebApiStandard.Contracts.RequestModels.Media
{
    public class GetVideoDetailRequest : IRequest<GetVideoDetailResponse>
    {
        public string VideoSlug { get; set; } = string.Empty;
    }
}
