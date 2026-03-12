using STTB.WebApiStandard.Contracts.ResponseModels.profiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Profiles
{
    public class GetAllAdministratorResponse
    {
       public IReadOnlyList<AdministratorDTO> Items { get; set; } = Array.Empty<AdministratorDTO>();
    }
    public class AdministratorDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
