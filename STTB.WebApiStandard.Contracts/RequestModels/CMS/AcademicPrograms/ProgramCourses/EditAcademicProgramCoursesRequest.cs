using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses
{
    public class EditAcademicProgramCoursesRequest : IRequest<EditAcademicProgramCoursesResponse>
    {
        public long Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
