using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicProgramCourse
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public int Credits { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AcademicCategoryCourse> AcademicCategoryCourses { get; set; } = new List<AcademicCategoryCourse>();
}
