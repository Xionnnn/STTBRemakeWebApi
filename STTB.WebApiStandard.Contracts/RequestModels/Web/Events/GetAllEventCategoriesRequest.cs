using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Events;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.Events
{
    public class GetAllEventCategoriesRequest : IRequest<GetAllEventCategoriesResponse>
    {
    }
}
