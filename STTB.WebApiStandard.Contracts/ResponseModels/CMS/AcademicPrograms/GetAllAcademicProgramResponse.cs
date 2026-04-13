using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.CMS.AcademicPrograms;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms
{
    public class GetAllAcademicProgramResponse
    {
        public IReadOnlyList<AcademicProgramDTO> Items { get; set; } = Array.Empty<AcademicProgramDTO>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
    