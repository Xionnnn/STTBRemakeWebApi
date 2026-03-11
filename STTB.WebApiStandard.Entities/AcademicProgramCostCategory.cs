using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicProgramCostCategory
{
    public long Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AcademicProgramCostCategoryMap> AcademicProgramCostCategoryMaps { get; set; } = new List<AcademicProgramCostCategoryMap>();
}
