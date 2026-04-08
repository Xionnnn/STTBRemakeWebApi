using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories
{
    public class AddNewsCategoryRequest : IRequest<AddNewsCategoryResponse>
    {
        public string CategoryName { get; set; } = string.Empty;
    }
}
