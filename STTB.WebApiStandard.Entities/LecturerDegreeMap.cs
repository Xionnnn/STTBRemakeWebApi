using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class LecturerDegreeMap
{
    public long LecturerId { get; set; }

    public long LecturerDegreeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Lecturer Lecturer { get; set; } = null!;

    public virtual LecturerDegree LecturerDegree { get; set; } = null!;
}
