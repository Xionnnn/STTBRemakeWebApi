using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.Media
{
    public class GetArticleDetailRequest : IRequest<GetArticleDetailResponse>
    {
        public string ArticleSlug { get; set; } = string.Empty;
    }
}
