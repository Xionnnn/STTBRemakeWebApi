using MediatR;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf
{
    public class DeleteMediaMonografRequest : IRequest
    {
        public long Id { get; set; }
    }
}
