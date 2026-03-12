using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.News;

namespace STTB.WebApiStandard.Contracts.RequestModels.News
{
    public class GetAllNewsCategoriesRequest : IRequest<GetAllNewsCategoriesResponse>
    {
    }
}
