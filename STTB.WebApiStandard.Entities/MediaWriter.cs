using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaWriter
{
    public long Id { get; set; }

    public string AuthorName { get; set; } = null!;

    public string? AuthorPosition { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<MediaItemWriter> MediaItemWriters { get; set; } = new List<MediaItemWriter>();
}
