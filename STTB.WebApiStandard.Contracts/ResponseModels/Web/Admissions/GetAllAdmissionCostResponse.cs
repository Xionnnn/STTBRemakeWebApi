using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.Web.Admissions;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Admissions
{
    public class GetAllAdmissionCostResponse
    {
        public IReadOnlyList<ProgramCostDTO> Items { get; set; } = Array.Empty<ProgramCostDTO>();
    }
}
