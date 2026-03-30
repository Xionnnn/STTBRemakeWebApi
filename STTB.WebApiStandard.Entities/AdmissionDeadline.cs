using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AdmissionDeadline
{
    public long Id { get; set; }

    public string AcademicYear { get; set; } = null!;

    public int BatchOrder { get; set; }

    public DateTime BatchDeadlineAt { get; set; }

    public DateTime FormReturnDeadlineAt { get; set; }

    public DateTime DocumentSelectionDeadlineAt { get; set; }

    public DateTime ResultBroadcastAt { get; set; }

    public DateTime ParticipantCallAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
