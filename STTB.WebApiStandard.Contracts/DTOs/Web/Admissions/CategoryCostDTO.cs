using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Admissions
{
    public class CategoryCostDTO
    {
            public string CategoryName { get; set; } = string.Empty;
            public IReadOnlyList<IndividualCostDTO> CostBreakdown { get; set; } = Array.Empty<IndividualCostDTO>();
    }
}
