using System;
using System.Collections.Generic;
using System.Text;

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
    public class AcademicProgramDTO
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public int? Duration { get; set; } = null;
        public int? TotalCredit { get; set; } = null;
        public string IsPublished { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
    