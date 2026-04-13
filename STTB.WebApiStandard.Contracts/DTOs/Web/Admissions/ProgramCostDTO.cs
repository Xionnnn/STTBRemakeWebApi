using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Admissions
{
    public class ProgramCostDTO
    {
        public int Id { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public IReadOnlyList<CategoryCostDTO> CostCategory { get; set; } = Array.Empty<CategoryCostDTO>();
    }
}
