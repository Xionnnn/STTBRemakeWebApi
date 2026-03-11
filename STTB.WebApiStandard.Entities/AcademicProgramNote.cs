using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class AcademicProgramNote
{
    public long Id { get; set; }

    public long ProgramId { get; set; }

    public string NoteText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual AcademicProgram Program { get; set; } = null!;
}
