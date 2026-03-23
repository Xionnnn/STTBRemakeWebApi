using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts
{
    public class GetAllCostResponse
    {
        public IReadOnlyList<CostDto> Items { get; set; } = Array.Empty<CostDto>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
    public class CostDto
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string CostName { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
