using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Events
{
    public class GetAllEventCategoriesRequest : IRequest<GetAllEventCategoriesResponse>
    {
    }
}
