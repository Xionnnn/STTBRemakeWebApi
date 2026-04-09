using STTB.WebApiStandard.Contracts.DTOs.CMS.AcademicCourses;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms.ProgramCourses
{
    public class GetAllAcademicProgramCoursesResponse
    {
        public List<GetAllAcademicCourseDTO> Items { get; set; } = new List<GetAllAcademicCourseDTO>();

        //pagination meta data
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
