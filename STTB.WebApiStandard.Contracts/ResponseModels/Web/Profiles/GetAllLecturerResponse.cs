using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.Web.Profiles;

namespace STTB.WebApiStandard.Contracts.ResponseModels.profiles
{
    public class GetAllLecturerResponse
    {
        public IReadOnlyList<LecturerDto> Items { get; set; } = Array.Empty<LecturerDto>();
    }
}
