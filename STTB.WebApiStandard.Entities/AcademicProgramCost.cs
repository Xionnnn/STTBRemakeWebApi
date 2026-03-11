using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicProgramCost
{
    public long Id { get; set; }

    public long AcademicProgramId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AcademicProgram AcademicProgram { get; set; } = null!;

    public virtual ICollection<AcademicProgramCostCategoryMap> AcademicProgramCostCategoryMaps { get; set; } = new List<AcademicProgramCostCategoryMap>();
}
