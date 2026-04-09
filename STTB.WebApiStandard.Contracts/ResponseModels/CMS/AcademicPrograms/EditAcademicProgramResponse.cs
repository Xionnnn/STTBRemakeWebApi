using STTB.WebApiStandard.Contracts.DTOs.CMS.AcademicCourses;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms
{
    public class EditAcademicProgramResponse
    {
        public long Id { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ProgramDescription { get; set; } = string.Empty;
        public List<string> ProgramRequirements { get; set; } = new List<string>();
        public int? TotalCredits { get; set; } = null;
        public int? Duration { get; set; } = null;
        public List<string> Notes { get; set; } = new List<string>();
        public List<string> LecturingSystem { get; set; } = new List<string>();
        public string Degree { get; set; } = string.Empty;
        public string Motto { get; set; } = string.Empty;
        public string InformedDescription { get; set; } = string.Empty;
        public string TransformedDescription { get; set; } = string.Empty;
        public string TransformativeDescription { get; set; } = string.Empty;
        public List<AcademicCategoryDTO> CourseCategory { get; set; } = new List<AcademicCategoryDTO>();
        public bool IsPublished { get; set; } = false;
    }
}
