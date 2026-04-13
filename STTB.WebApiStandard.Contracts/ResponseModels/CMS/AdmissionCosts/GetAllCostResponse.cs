using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.CMS.AdmissionCosts;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts
{
    public class GetAllCostResponse
    {
        public IReadOnlyList<CostDTO> Items { get; set; } = Array.Empty<CostDTO>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
