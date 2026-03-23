using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts
{
    public class GetCostResponse
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string CostName { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
