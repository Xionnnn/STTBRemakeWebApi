using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Articles;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles
{
    public class GetMediaArticleRequest : IRequest<GetMediaArticleResponse>
    {
        public long Id { get; set; }
    }
}
