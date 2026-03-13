using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Admissions
{
    public class GetAllAdmissionCostResponse
    {
        public IReadOnlyList<ProgramCostDTO> Items { get; set; } = Array.Empty<ProgramCostDTO>();
    }
    public class ProgramCostDTO
    {
        public int Id { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string Slug { get; set; }
        public IReadOnlyList<CategoryCostDTO> CostCategory { get; set; } = Array.Empty<CategoryCostDTO>();
    }
    public class CategoryCostDTO
    {
            public string CategoryName { get; set; } = string.Empty;
            public IReadOnlyList<IndividualCostDTO> CostBreakdown { get; set; } = Array.Empty<IndividualCostDTO>();
    }
    public class  IndividualCostDTO
    {
        public string CostName { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
