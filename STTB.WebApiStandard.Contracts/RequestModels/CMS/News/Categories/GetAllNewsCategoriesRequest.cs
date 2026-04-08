using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories
{
    public class GetAllNewsCategoriesRequest : IRequest<GetAllNewsCategoriesResponse>
    {
        public string CategoryName { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool FetchAll { get; set; } = false;
    }
}
