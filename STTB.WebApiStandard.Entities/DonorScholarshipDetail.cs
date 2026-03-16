using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class DonorScholarshipDetail
{
    public long Id { get; set; }

    public long DonorMemberId { get; set; }

    public string StudentName { get; set; } = null!;

    public long AcademicProgramId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AcademicProgram AcademicProgram { get; set; } = null!;

    public virtual DonorMember DonorMember { get; set; } = null!;
}
