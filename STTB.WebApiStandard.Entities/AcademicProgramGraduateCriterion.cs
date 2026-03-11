using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicProgramGraduateCriterion
{
    public long Id { get; set; }

    public long ProgramId { get; set; }

    public string CriterionName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AcademicProgram Program { get; set; } = null!;
}
