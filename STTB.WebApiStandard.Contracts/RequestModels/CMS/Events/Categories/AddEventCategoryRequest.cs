using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories
{
    public class AddEventCategoryRequest : IRequest<AddEventCategoryResponse>
    {
        public string CategoryName { get; set; } = string.Empty;
    }
}
