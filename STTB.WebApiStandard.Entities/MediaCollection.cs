using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaCollection
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<MediaItem> MediaItems { get; set; } = new List<MediaItem>();
}
