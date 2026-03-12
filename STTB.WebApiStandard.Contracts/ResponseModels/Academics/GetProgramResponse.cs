using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Academic
{
    public class GetProgramResponse
    {
        public string ProgramName { get; set; } = string.Empty;
        public string ProgramDescription { get; set; } = string.Empty;
        public IReadOnlyList<string> ProgramRequirements = Array.Empty<string>();
        public int? TotalCredits = null;
        public string Duration = string.Empty;
        public IReadOnlyList<string> Notes = Array.Empty<string>();
        public IReadOnlyList<string> LecturingSystem = Array.Empty<string>();
        public string Degree = string.Empty;
        public string Motto = string.Empty;
        public string InformedDescription = string.Empty;
        public string TransformedDescription = string.Empty;
        public string TransformativeDescription = string.Empty;


    }
    public class AcademicDTO
    {
        public string CategoryName = string.Empty;
        public int? TotalCredits = null;
        public IReadOnlyList<LectureDTO> Lectures = Array.Empty<LectureDTO>();


    }
    public class LectureDTO
    {
        public string LectureName = string.Empty;
        public int? Credits = null;
        public string Description = string.Empty;

    }
}
