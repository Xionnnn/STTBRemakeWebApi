using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class MediaItemTopic
{
    public long MediaItemId { get; set; }

    public long TopicCategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual MediaItem MediaItem { get; set; } = null!;

    public virtual MediaTopicCategory TopicCategory { get; set; } = null!;
}
