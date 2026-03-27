using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers
{
    public class GetAllLecturerResponse
    {
        public IReadOnlyList<LecturerDto> Items { get; set; } = Array.Empty<LecturerDto>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

    }
    public class LecturerDto
    {
        public long Id { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string OrganizationalRole { get; set; } = string.Empty;
        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Degrees { get; set; } = Array.Empty<string>();
        public bool IsActive { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LecturerImagePath { get; set; } = string.Empty;
    }
}
