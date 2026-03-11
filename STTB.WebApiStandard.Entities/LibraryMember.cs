using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class LibraryMember
{
    public long Id { get; set; }

    public string FullName { get; set; } = null!;

    public string InstitutionName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
