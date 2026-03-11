using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicCourse
{
    public long Id { get; set; }

    public long CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public int Credits { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AcademicCourseCategory Category { get; set; } = null!;
}
