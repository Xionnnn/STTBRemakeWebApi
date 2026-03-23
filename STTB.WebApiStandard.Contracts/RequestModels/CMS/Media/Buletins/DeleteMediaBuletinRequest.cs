using MediatR;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins
{
    public class DeleteMediaBuletinRequest : IRequest
    {
        public long Id { get; set; }
    }
}
