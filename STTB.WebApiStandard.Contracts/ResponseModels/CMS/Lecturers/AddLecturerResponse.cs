using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers
{
    public class AddLecturerResponse
    {
        public long Id { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string OrganizationalRole { get; set; } = string.Empty;
        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Degrees { get; set; } = Array.Empty<string>();
        public bool IsActive { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
