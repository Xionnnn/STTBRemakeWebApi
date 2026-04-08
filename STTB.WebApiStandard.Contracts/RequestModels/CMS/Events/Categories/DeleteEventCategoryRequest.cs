using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories
{
    public class DeleteEventCategoryRequest : IRequest<DeleteEventCategoryResponse>
    {
        public long Id { get; set; }
    }
}
