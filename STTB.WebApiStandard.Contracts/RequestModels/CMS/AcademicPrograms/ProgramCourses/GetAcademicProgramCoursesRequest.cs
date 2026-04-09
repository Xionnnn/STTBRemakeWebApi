using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses
{
    public class GetAcademicProgramCoursesRequest : IRequest<GetAcademicProgramCoursesResponse>
    {
        public long Id { get; set; }
    }
}
