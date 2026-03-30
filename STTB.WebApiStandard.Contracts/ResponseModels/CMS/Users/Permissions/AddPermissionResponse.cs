using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Permissions
{
    public class AddPermissionResponse
    {
        public long Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
    }
}
