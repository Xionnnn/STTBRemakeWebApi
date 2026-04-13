using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.Lecturers
{
    public class LecturerDTO
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
