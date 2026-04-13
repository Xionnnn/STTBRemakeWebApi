using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.Web.Admissions;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Admissions
{
    public class GetAdmissionScheduleResponse
    {
        public List<AdmissionScheduleDTO> Items { get; set; } = new List<AdmissionScheduleDTO>();

    }
}
