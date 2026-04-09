using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.AcademicCourses
{
    public class AcademicCategoryDTO
    {
        public string CategoryName { get; set; } = string.Empty;
        public int? TotalCredits { get; set; } = null;
        public List<AcademicCourseDTO> Courses { get; set; } = new List<AcademicCourseDTO>();
    }
}
