using System;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.Users.Permissions
{
    public class PermissionDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
