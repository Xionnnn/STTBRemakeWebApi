using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class LecturerRoleMap
{
    public long LecturerId { get; set; }

    public long LecturerRoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Lecturer Lecturer { get; set; } = null!;

    public virtual LecturerRole LecturerRole { get; set; } = null!;
}
