using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Academics
{
    public class GetAllAcademicDTO
    {
        public long ProgramId { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public int? Duration { get; set; } = null;
        public int? TotalCredit { get; set; } = null;
    }
}
