using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Events;

namespace STTB.WebApiStandard.Contracts.RequestModels.Events
{
    public class GetAllEventCategoriesRequest : IRequest<GetAllEventCategoriesResponse>
    {
    }
}
