using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaItemWriter
{
    public long MediaItemId { get; set; }

    public long MediaWriterId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual MediaItem MediaItem { get; set; } = null!;

    public virtual MediaWriter MediaWriter { get; set; } = null!;
}
