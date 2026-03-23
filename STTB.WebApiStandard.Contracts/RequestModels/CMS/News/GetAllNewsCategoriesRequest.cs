using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News
{
    public class GetAllNewsCategoriesRequest : IRequest<GetAllNewsCategoriesResponse>
    {
    }
}
