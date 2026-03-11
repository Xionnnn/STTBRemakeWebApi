using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AdmissionDeadline
{
    public long Id { get; set; }

    public string AcademicYear { get; set; } = null!;

    public DateTime FirstBatchClosingAt { get; set; }

    public DateTime SecondBatchClosingAt { get; set; }

    public DateTime ThirdBatchClosingAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
