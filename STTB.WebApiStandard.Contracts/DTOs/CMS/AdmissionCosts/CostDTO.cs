using System;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.AdmissionCosts
{
    public class CostDTO
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string CostName { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
