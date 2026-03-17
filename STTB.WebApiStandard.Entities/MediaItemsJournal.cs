using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaItemsJournal
{
    public long Id { get; set; }

    public long MediaItemId { get; set; }

    public string? Issn { get; set; }

    public string? EIssn { get; set; }

    public string? Doi { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual MediaItem MediaItem { get; set; } = null!;
}
