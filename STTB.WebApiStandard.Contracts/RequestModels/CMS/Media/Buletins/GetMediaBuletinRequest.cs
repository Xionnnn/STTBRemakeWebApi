using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Buletins;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins
{
    public class GetMediaBuletinRequest : IRequest<GetMediaBuletinResponse>
    {
        public long Id { get; set; }
    }
}
