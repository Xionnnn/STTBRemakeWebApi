using System;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Admissions
{
    public class IndividualCostDTO
    {
        public string CostName { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
