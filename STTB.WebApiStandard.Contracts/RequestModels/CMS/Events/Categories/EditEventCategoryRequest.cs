using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories
{
    public class EditEventCategoryRequest : IRequest<EditEventCategoryResponse>
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
