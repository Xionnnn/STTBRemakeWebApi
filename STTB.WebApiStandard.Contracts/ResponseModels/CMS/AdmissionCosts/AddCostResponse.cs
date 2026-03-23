using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts
{
    public class AddCostResponse
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string CostName { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
