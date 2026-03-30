using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionDeadline;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline
{
    public class EditBatchDeadlineRequest : IRequest<EditBatchDeadlineResponse>
    {
        public long Id { get; set; }
        public string AcademicYear { get; set; } = string.Empty;
        public int BatchOrder { get; set; }
        public DateTime BatchDeadlineAt { get; set; }
        public DateTime FormReturnDeadlineAt { get; set; }
        public DateTime DocumentSelectionDeadlineAt { get; set; }
        public DateTime ResultBroadcastAt { get; set; }
        public DateTime ParticipantCallAt { get; set; }
        public bool IsActive { get; set; }
    }
}
