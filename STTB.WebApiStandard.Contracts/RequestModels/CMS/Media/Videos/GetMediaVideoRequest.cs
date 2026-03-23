using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Videos;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Videos
{
    public class GetMediaVideoRequest : IRequest<GetMediaVideoResponse>
    {
        public long Id { get; set; }
    }
}
