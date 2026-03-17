using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaItemsMonograf
{
    public long Id { get; set; }

    public long MediaItemId { get; set; }

    public decimal? Price { get; set; }

    public string? Contact { get; set; }

    public string? Isbn { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual MediaItem MediaItem { get; set; } = null!;
}
