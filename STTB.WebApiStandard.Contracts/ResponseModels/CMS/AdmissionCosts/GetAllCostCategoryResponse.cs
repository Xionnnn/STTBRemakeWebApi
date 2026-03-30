using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts
{
    public class GetAllCostCategoryResponse
    {
        public List<CMSCostCategoryDTO> Items { get; set; } = new List<CMSCostCategoryDTO>();
    }
    public class CMSCostCategoryDTO
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}