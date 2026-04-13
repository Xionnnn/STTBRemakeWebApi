using System;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Admissions
{
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
