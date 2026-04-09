using STTB.WebApiStandard.Contracts.DTOs.Web.Academics;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Academic
{
    public class GetAcademicProgramResponse
    {
        public string ProgramName { get; set; } = string.Empty;
        public string ProgramDescription { get; set; } = string.Empty;
        public List<string> ProgramRequirements { get; set; } = new List<string>();
        public int? TotalCredits { get; set; } = null;
        public string Duration { get; set; } = string.Empty;
        public List<string> Notes { get; set; } = new List<string>();
        public List<string> LecturingSystem { get; set; } = new List<string>();
        public string Degree { get; set; } = string.Empty;
        public string Motto { get; set; } = string.Empty;
        public string InformedDescription { get; set; } = string.Empty;
        public string TransformedDescription { get; set; } = string.Empty;
        public string TransformativeDescription { get; set; } = string.Empty;
        public List<AcademicCategoryDTO> ProgramCategory { get; set; } = new List<AcademicCategoryDTO>();
    }

}
