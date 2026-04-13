using System;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.AdmissionCosts.Categories
{
    public class CMSCostCategoryDTO
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
