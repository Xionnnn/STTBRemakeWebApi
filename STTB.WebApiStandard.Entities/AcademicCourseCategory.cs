using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicCourseCategory
{
    public long Id { get; set; }

    public long ProgramId { get; set; }

    public string Name { get; set; } = null!;

    public int TotalCredits { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AcademicCourse> AcademicCourses { get; set; } = new List<AcademicCourse>();

    public virtual AcademicProgram Program { get; set; } = null!;
}
