using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class FoundationAdministrator
{
    public long Id { get; set; }

    public string AdminName { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string? Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
