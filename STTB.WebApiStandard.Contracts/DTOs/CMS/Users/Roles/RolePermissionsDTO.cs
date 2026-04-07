using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.Users.Roles
{
    public class RolePermissionsDTO
    {
        public long Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
    }
}
