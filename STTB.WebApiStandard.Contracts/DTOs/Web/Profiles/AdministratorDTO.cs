using System;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Profiles
{
    public class AdministratorDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
