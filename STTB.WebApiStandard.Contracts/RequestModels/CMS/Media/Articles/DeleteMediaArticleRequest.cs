using MediatR;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles
{
    public class DeleteMediaArticleRequest : IRequest
    {
        public long Id { get; set; }
    }
}
