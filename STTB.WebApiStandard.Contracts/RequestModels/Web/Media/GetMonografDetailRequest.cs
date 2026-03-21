using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.Media
{
    public class GetMonografDetailRequest : IRequest<GetMonografDetailResponse>
    {
        public string MonografSlug { get; set; } = string.Empty;
    }
}
