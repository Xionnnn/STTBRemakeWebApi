using STTB.WebApiStandard.Contracts.DTOs.CMS.Lecturers;
using System;
using System.Collections.Generic;
using System.Text;


namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers
{
    public class GetAllLecturerResponse
    {
        public IReadOnlyList<LecturerDTO> Items { get; set; } = Array.Empty<LecturerDTO>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

    }
}
