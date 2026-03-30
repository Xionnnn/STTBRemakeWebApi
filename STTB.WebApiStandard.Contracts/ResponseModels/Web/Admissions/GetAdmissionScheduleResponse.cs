using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Admissions
{
    public class GetAdmissionScheduleResponse
    {
        public List<AdmissionScheduleDTO> Items { get; set; } = new List<AdmissionScheduleDTO>();

    }
    public class AdmissionScheduleDTO
    {
        public string AcademicYear { get; set; } = string.Empty;
        public int BatchOrder { get; set; }
        public DateTime BatchDeadlineAt { get; set; }
        public DateTime FormReturnDeadlineAt { get; set; }
        public DateTime DocumentSelectionDeadlineAt { get; set; }
        public DateTime ResultBroadcastAt { get; set; }
        public DateTime ParticipantCallAt { get; set; }
    }
}
