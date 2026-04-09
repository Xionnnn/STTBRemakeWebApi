using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicCategoryCourse
{
    public long AcademicProgramCourseId { get; set; }

    public long AcademicProgramCategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AcademicProgramCategory AcademicProgramCategory { get; set; } = null!;

    public virtual AcademicProgramCourse AcademicProgramCourse { get; set; } = null!;
}
