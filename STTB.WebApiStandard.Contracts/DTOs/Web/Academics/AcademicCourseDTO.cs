using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Academics
{
    public class AcademicCourseDTO
    {
        public string CourseName { get; set; } = string.Empty;
        public int? Credits { get; set; } = null;
        public string Description { get; set; } = string.Empty;
    }
}
