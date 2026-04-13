using System;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.AcademicPrograms
{
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
