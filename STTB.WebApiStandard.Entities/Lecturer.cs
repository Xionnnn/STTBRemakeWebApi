using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class Lecturer
{
    public long Id { get; set; }

    public string LecturerName { get; set; } = null!;

    public DateTime JoinedAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<LecturerDegreeMap> LecturerDegreeMaps { get; set; } = new List<LecturerDegreeMap>();

    public virtual ICollection<LecturerRoleMap> LecturerRoleMaps { get; set; } = new List<LecturerRoleMap>();
}
