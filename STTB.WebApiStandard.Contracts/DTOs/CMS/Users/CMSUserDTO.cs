using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.Users
{
    public class CMSUserDTO
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}
