using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.Users.Roles
{
    public class RoleDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> RolePermissions { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
    }
}
