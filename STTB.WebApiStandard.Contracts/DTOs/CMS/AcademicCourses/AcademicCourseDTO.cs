using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.AcademicCourses
{
    public class AcademicCourseDTO
    {
        public long Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int? Credits { get; set; } = null;
        public string Description { get; set; } = string.Empty;
    }
}
