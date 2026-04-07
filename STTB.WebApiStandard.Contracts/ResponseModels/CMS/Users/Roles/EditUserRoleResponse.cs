using STTB.WebApiStandard.Contracts.DTOs.CMS.Users.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Roles
{
    public class EditUserRoleResponse
    {
        public long Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<RolePermissionsDTO> RolePermissions { get; set; } = new List<RolePermissionsDTO>();
    }
}
