using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaItem
{
    public long Id { get; set; }

    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string MediaFormat { get; set; } = null!;

    public string? Description { get; set; }

    public string? Content { get; set; }

    public DateTime PublishedAt { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<MediaItemTopic> MediaItemTopics { get; set; } = new List<MediaItemTopic>();

    public virtual ICollection<MediaItemWriter> MediaItemWriters { get; set; } = new List<MediaItemWriter>();

    public virtual MediaItemsJournal? MediaItemsJournal { get; set; }

    public virtual MediaItemsMonograf? MediaItemsMonograf { get; set; }

    public virtual MediaItemsVideo? MediaItemsVideo { get; set; }
}
