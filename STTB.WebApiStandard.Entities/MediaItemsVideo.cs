using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaItemsVideo
{
    public long Id { get; set; }

    public long MediaItemId { get; set; }

    public string VideoUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual MediaItem MediaItem { get; set; } = null!;
}
