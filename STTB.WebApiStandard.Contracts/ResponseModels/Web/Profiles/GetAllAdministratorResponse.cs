using STTB.WebApiStandard.Contracts.ResponseModels.profiles;
using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.Web.Profiles;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Profiles
{
    public class GetAllAdministratorResponse
    {
       public IReadOnlyList<AdministratorDTO> Items { get; set; } = Array.Empty<AdministratorDTO>();
    }
}
