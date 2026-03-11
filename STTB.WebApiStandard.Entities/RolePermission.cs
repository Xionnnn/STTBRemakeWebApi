using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class RolePermission
{
    public long RoleId { get; set; }

    public long PermissionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
