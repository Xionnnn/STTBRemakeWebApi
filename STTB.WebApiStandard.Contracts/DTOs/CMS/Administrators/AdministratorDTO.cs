using System;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.Administrators
{
    public class AdministratorDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
