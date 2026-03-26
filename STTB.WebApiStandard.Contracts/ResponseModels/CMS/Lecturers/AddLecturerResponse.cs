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
        public List<string> Roles { get; set; } = new List<string>(); 
        public List<string> Degrees { get; set; } = new List<string>();
        public bool IsActive { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
