using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms
{
    public class GetAcademicProgramResponse
    {
        public long Id { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string ProgramDescription { get; set; } = string.Empty;
        public IReadOnlyList<string> ProgramRequirements { get; set; } = Array.Empty<string>();
        public int? TotalCredits { get; set; } = null;
        public int? Duration { get; set; } = null;
        public IReadOnlyList<string> Notes { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> LecturingSystem { get; set; } = Array.Empty<string>();
        public string Degree { get; set; } = string.Empty;
        public string Motto { get; set; } = string.Empty;
        public string InformedDescription { get; set; } = string.Empty;
        public string TransformedDescription { get; set; } = string.Empty;
        public string TransformativeDescription { get; set; } = string.Empty;
        public IReadOnlyList<AcademicDTO> LectureCategory { get; set; } = Array.Empty<AcademicDTO>();
    }
    public class AcademicDTO
    {
        public string CategoryName { get; set; } = string.Empty;
        public int? TotalCredits { get; set; } = null;
        public IReadOnlyList<LectureDTO> Lectures { get; set; } = Array.Empty<LectureDTO>();


    }
    public class LectureDTO
    {
        public string LectureName { get; set; } = string.Empty;
        public int? Credits { get; set; } = null;
        public string Description { get; set; } = string.Empty;

    }
}
