using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses
{
    public class EditAcademicProgramCoursesResponse
    {
        public long Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
