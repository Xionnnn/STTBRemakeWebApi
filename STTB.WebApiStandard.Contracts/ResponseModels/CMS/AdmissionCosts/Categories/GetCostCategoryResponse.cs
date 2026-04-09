using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories
{
    public class GetCostCategoryResponse
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
