using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles
{
    public class EditUserRoleRequest : IRequest<EditUserRoleResponse>
    {
        public long Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<string> RolePermissions { get; set; } = new List<string>();
    }
}
