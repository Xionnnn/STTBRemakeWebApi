using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class LecturerDegree
{
    public long Id { get; set; }

    public string DegreeName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<LecturerDegreeMap> LecturerDegreeMaps { get; set; } = new List<LecturerDegreeMap>();
}
