using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses
{
    public class GetAllAcademicProgramCoursesRequest : IRequest<GetAllAcademicProgramCoursesResponse>
    {
        public string CourseName { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool FetchAll { get; set; } = false;
    }
}
