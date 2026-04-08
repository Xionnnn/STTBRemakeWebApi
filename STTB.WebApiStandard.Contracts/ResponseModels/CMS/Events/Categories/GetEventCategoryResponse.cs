namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events.Categories
{
    public class GetEventCategoryResponse
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
