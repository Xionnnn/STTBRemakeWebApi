using STTB.WebApiStandard.Contracts.DTOs.CMS.Events;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories
{
    public class GetAllEventCategoriesResponse
    {
        public List<GetAllEventCategoriesDTO> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
