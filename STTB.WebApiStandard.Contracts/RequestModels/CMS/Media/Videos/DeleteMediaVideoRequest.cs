using MediatR;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Videos
{
    public class DeleteMediaVideoRequest : IRequest
    {
        public long Id { get; set; }
    }
}
