using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Monograf;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf
{
    public class GetMediaMonografRequest : IRequest<GetMediaMonografResponse>
    {
        public long Id { get; set; }
    }
}
