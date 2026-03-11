using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class LecturerRole
{
    public long Id { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<LecturerRoleMap> LecturerRoleMaps { get; set; } = new List<LecturerRoleMap>();
}
