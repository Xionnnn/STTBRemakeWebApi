using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaItem
{
    public long Id { get; set; }

    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string MediaFormat { get; set; } = null!;

    public long? CollectionId { get; set; }

    public string? Description { get; set; }

    public string? Content { get; set; }

    public string? VideoUrl { get; set; }

    public string? Theme { get; set; }

    public string? AuthorName { get; set; }

    public string? AuthorPosition { get; set; }

    public DateTime PublishedAt { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual MediaCollection? Collection { get; set; }

    public virtual ICollection<MediaItemTopic> MediaItemTopics { get; set; } = new List<MediaItemTopic>();
}
