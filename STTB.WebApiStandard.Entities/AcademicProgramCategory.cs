using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicProgramCategory
{
    public long Id { get; set; }

    public long ProgramId { get; set; }

    public string Name { get; set; } = null!;

    public int TotalCredits { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AcademicCategoryCourse> AcademicCategoryCourses { get; set; } = new List<AcademicCategoryCourse>();

    public virtual AcademicProgram Program { get; set; } = null!;
}
