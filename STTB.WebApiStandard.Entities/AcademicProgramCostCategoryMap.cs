using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicProgramCostCategoryMap
{
    public long AcademicProgramCostId { get; set; }

    public long AcademicProgramCostCategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AcademicProgramCost AcademicProgramCost { get; set; } = null!;

    public virtual AcademicProgramCostCategory AcademicProgramCostCategory { get; set; } = null!;
}
